using apbd_project.Model.Dto;
using apbd_project.Service;
using Microsoft.AspNetCore.Mvc;

namespace apbd_project.Controller;

[Route("api/discounts")]
[ApiController]
public class DiscountsController : ControllerBase
{
    private readonly IDiscountService _discountService;

    public DiscountsController(IDiscountService discountService)
    {
        _discountService = discountService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDiscountById(int id)
    {
        var discount = await _discountService.GetDiscountById(id);

        if (discount == null)
        {
            return NotFound();
        }

        return Ok(discount);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDiscounts()
    {
        var discounts = await _discountService.GetAllDiscounts();
        return Ok(discounts);
    }

    [HttpPost]
    public async Task<IActionResult> AddDiscount(AddDiscountDto addDiscountDto)
    {
        await _discountService.AddDiscount(addDiscountDto);
        return Created();
    }
}