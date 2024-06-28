using apbd_project.Data;
using apbd_project.Model;
using apbd_project.Model.Dto;
using Microsoft.EntityFrameworkCore;

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

    public async Task<bool> DoesContractExist(int id)
    {
        return await _context.Contracts.AnyAsync(c => c.Id == id);
    }

    public async Task<bool> IsContractDurationValid(DateTime startDate, DateTime endDate)
    {
        return (endDate - startDate).Days >= 3 && (endDate - startDate).Days <= 30;
    }

    public async Task<bool> DoesClientHaveAnyContracts(int id)
    {
        return await _context.Contracts.AnyAsync(c => c.ClientId == id);
    }

    public async Task<bool> DoesClientHaveContractForSoftwareProduct(int clientId, int softwareProductId)
    {
        return await _context.Contracts.AnyAsync(c => c.ClientId == clientId && c.SoftwareProductId == softwareProductId);
    }

    public async Task<int> AddOneTimePurchaseContract(AddOneTimePurchaseContractDto addContractDto)
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
            StartDate = addContractDto.StartDate
        };

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        // Create the one-time purchase
        var oneTimePurchase = new OneTimePurchase
        {
            ContractId = contract.Id,
            EndDate = addContractDto.EndDate,
            Version = addContractDto.Version,
            SupportEndDate = addContractDto.SupportEndDate
        };

        contract.OneTimePurchase = oneTimePurchase;

        await _context.OneTimePurchases.AddAsync(oneTimePurchase);
        await _context.SaveChangesAsync();

        return contract.Id;
    }
}