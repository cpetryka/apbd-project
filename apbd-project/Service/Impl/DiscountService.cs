using apbd_project.Data;
using apbd_project.Model;
using apbd_project.Model.Dto;
using Microsoft.EntityFrameworkCore;

namespace apbd_project.Service.Impl;

public class DiscountService : IDiscountService
{
    private readonly ApplicationContext _context;

    public DiscountService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<GetDiscountDto?> GetDiscountById(int id)
    {
        return await _context
            .Discounts
            .Where(d => d.Id == id)
            .Select(d => new GetDiscountDto
            {
                Id = d.Id,
                Name = d.Name,
                Offer = d.Offer,
                Amount = d.Amount,
                StartDate = d.StartDate,
                EndDate = d.EndDate
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<GetDiscountDto>> GetAllDiscounts()
    {
        return await _context
            .Discounts
            .Select(d => new GetDiscountDto
            {
                Id = d.Id,
                Name = d.Name,
                Offer = d.Offer,
                Amount = d.Amount,
                StartDate = d.StartDate,
                EndDate = d.EndDate
            })
            .ToListAsync();
    }

    public async Task AddDiscount(AddDiscountDto addDiscountDto)
    {
        var discount = new Discount
        {
            Name = addDiscountDto.Name,
            Offer = addDiscountDto.Offer,
            Amount = addDiscountDto.Amount,
            StartDate = addDiscountDto.StartDate,
            EndDate = addDiscountDto.EndDate
        };

        await _context.Discounts.AddAsync(discount);
        await _context.SaveChangesAsync();
    }

    public async Task<GetDiscountDto?> FindCurrentlyBestDiscount()
    {
        var now = DateTime.Now;

        return await _context
            .Discounts
            .Where(d => d.StartDate <= now && d.EndDate >= now)
            .OrderByDescending(d => d.Amount)
            .Select(d => new GetDiscountDto
            {
                Id = d.Id,
                Name = d.Name,
                Offer = d.Offer,
                Amount = d.Amount,
                StartDate = d.StartDate,
                EndDate = d.EndDate
            })
            .FirstOrDefaultAsync();
    }
}