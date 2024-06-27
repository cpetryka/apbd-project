using apbd_project.Data;
using apbd_project.Model;
using apbd_project.Model.Dto;

namespace apbd_project.Service.Impl;

public class ContractService : IContractService
{
    private readonly ApplicationContext _context;
    private readonly IDiscountService _discountService;

    public ContractService(ApplicationContext context, IDiscountService discountService)
    {
        _context = context;
        _discountService = discountService;
    }

    public Task<bool> IsContractDurationValid(DateTime startDate, DateTime endDate)
    {
        return Task.FromResult((endDate - startDate).Days >= 3 && (endDate - startDate).Days <= 30);
    }

    public Task<bool> DoesClientHaveAnyContracts(int id)
    {
        return Task.FromResult(_context.Contracts.Any(c => c.ClientId == id));
    }

    public Task<bool> DoesClientHaveContractForSoftwareProduct(int clientId, int softwareProductId)
    {
        return Task.FromResult(_context.Contracts.Any(c => c.ClientId == clientId && c.SoftwareProductId == softwareProductId));
    }

    public Task<int> AddOneTimePurchaseContract(AddOneTimePurchaseContractDto addContractDto)
    {
        // Calculate the discount
        var bestDiscount = _discountService.FindCurrentlyBestDiscount().Result;
        var discount = bestDiscount != null ? bestDiscount.Amount : 0;

        if (DoesClientHaveAnyContracts(addContractDto.ClientId).Result)
        {
            discount += 5;
        }

        // Calculate the total price
        var totalPrice = addContractDto.Price - (addContractDto.Price * discount / 100);

        // Create the contract
        var contract = new Contract
        {
            ClientId = addContractDto.ClientId,
            SoftwareProductId = addContractDto.SoftwareProductId,
            Price = totalPrice,
            StartDate = addContractDto.StartDate,
            SignedDate = DateTime.Now
        };

        _context.Contracts.Add(contract);
        _context.SaveChanges();

        // Create the one-time purchase
        var oneTimePurchase = new OneTimePurchase
        {
            ContractId = contract.Id,
            EndDate = addContractDto.EndDate,
            Version = addContractDto.Version,
            SupportEndDate = addContractDto.SupportEndDate
        };

        contract.OneTimePurchase = oneTimePurchase;

        _context.OneTimePurchases.Add(oneTimePurchase);
        _context.SaveChanges();

        return Task.FromResult(contract.Id);
    }
}