using Application.Dtos.Order;

namespace Application.Interfaces.Services
{
    public interface IDiscountService
    {
        decimal CalculateDiscount(decimal subtotal, DiscountDto? discountDto);
    }
}
