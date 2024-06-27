using apbd_project.Data;
using apbd_project.Model;
using Microsoft.EntityFrameworkCore;

namespace apbd_project.Service.Impl;

public class PaymentService : IPaymentsService
{
    private readonly ApplicationContext _context;
    private readonly IContractService _contractService;
    private readonly ICurrencyService _currencyService;

    public PaymentService(ApplicationContext context, IContractService contractService, ICurrencyService currencyService)
    {
        _context = context;
        _contractService = contractService;
        _currencyService = currencyService;
    }

    public async Task<bool> IsPaidAmountValid(int contractId, decimal amount)
    {
        if (amount <= 0)
        {
            return false;
        }

        var contractPrice = await _context
            .Contracts
            .Where(c => c.Id == contractId)
            .Select(c => c.Price)
            .FirstOrDefaultAsync();

        var paidAmount = await _context
            .Payments
            .Where(p => p.ContractId == contractId)
            .SumAsync(p => p.Amount);

        return contractPrice - paidAmount >= amount;
    }

    public async Task<bool> IsPaymentDateValid(int contractId, DateTime paymentDate)
    {
        var contract = await _context
            .Contracts
            .Include(c => c.OneTimePurchase)
            .Where(c => c.Id == contractId)
            .FirstOrDefaultAsync();

        return contract != null && paymentDate >= contract.StartDate && paymentDate <= contract.OneTimePurchase.EndDate;
    }

    public Task<decimal> GetPaidAmount(int contractId)
    {
        return _context
            .Payments
            .Where(p => p.ContractId == contractId)
            .SumAsync(p => p.Amount);
    }

    public async Task<int> AddPaymentForOneTimePurchaseContract(int contractId, decimal amount)
    {
        var payment = new Payment
        {
            ContractId = contractId,
            Amount = amount,
            PaymentDate = DateTime.Now
        };

        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();

        if (await GetPaidAmount(contractId) == _context.Contracts.Find(contractId).Price)
        {
            var contract = await _context.Contracts.FindAsync(contractId);
            contract.SignedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        return payment.Id;
    }

    public async Task<decimal?> CalculateProfit(string currencyCode, int? softwareProductId)
    {
        // Find all payments
        var paymentsQueryable = _context
            .Payments
            .Include(p => p.Contract)
            .AsQueryable();

        // If softwareProductId is provided, filter payments by softwareProductId
        if (softwareProductId.HasValue)
        {
            paymentsQueryable = paymentsQueryable.Where(p => p.Contract.SoftwareProductId == softwareProductId);
        }

        var paymentsList = await paymentsQueryable.ToListAsync();

        // Calculate profit
        decimal totalProfit = 0;
        var currentExchangeRate = await _currencyService.GetCurrencyExchangeRate(currencyCode);

        foreach (var payment in paymentsList)
        {
            if (payment.Contract.SignedDate.HasValue)
            {
                totalProfit += payment.Amount;
            }
        }

        return totalProfit / currentExchangeRate;
    }

    public async Task<decimal?> CalculatePredictableProfit(string currencyCode, int? softwareProductId, int months)
    {
        /*var paymentsQueryable = _context
            .Payments
            .Include(p => p.Contract)
            .Include(p => p.Contract.OneTimePurchase)
            .AsQueryable();

        if (softwareProductId.HasValue)
        {
            paymentsQueryable = paymentsQueryable.Where(p => p.Contract.SoftwareProductId == softwareProductId);
        }

        var paymentsList = await paymentsQueryable.ToListAsync();

        decimal totalProfit = 0;
        var currentExchangeRate = await _currencyService.GetCurrencyExchangeRate(currencyCode);

        foreach (var payment in paymentsList)
        {
            if (payment.Contract.SignedDate.HasValue/* || payment.Contract.OneTimePurchase.EndDate <= DateTime.Now.AddMonths(months)#1#)
            {
                totalProfit += payment.Amount;
            }
        }

        var notSignedContracts = await _context
            .Contracts
            .Include(c => c.OneTimePurchase)
            .Where(c => c.SignedDate == null && c.OneTimePurchase.EndDate <= DateTime.Now.AddMonths(months))
            .ToListAsync();

        foreach (var contract in notSignedContracts)
        {
            totalProfit += contract.Price;
        }

        return totalProfit / currentExchangeRate;*/

        decimal totalProfit = CalculateProfit(currencyCode, softwareProductId).Result ?? 0;
        var currentExchangeRate = await _currencyService.GetCurrencyExchangeRate(currencyCode);

        var notSignedContractsQueryable = _context
            .Contracts
            .Include(c => c.OneTimePurchase)
            .Where(c => !c.SignedDate.HasValue && c.OneTimePurchase.EndDate <= DateTime.Now.AddMonths(months))
            .AsQueryable();

        if (softwareProductId.HasValue)
        {
            notSignedContractsQueryable = notSignedContractsQueryable.Where(c => c.SoftwareProductId == softwareProductId);
        }

        var notSignedContracts = await notSignedContractsQueryable.ToListAsync();

        foreach (var contract in notSignedContracts)
        {
            totalProfit += contract.Price / (currentExchangeRate ?? 1);
        }

        return totalProfit;
    }
}