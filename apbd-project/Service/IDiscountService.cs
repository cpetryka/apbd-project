using apbd_project.Model;
using apbd_project.Model.Dto;

namespace apbd_project.Service;

public interface IDiscountService
{
    Task<GetDiscountDto?> GetDiscountById(int id);
    Task<IEnumerable<GetDiscountDto>> GetAllDiscounts();
    Task AddDiscount(AddDiscountDto addDiscountDto);
    Task<GetDiscountDto?> FindCurrentlyBestDiscount();
}