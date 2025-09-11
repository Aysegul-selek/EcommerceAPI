using Application.Dtos.Order;
using Application.Dtos.ResponseDto;

namespace Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<ApiResponseDto<OrderDto>> CreateStubOrderAsync(CreateOrderDto request, long userId, string? idempotencyKey = null);
        Task<ApiResponseDto<OrderDto?>> GetByIdAsync(long id);
        Task<ApiResponseDto<List<OrderDto>>> GetOrdersAsync();              // Tüm siparişler
        Task<ApiResponseDto<List<OrderDto>>> GetOrdersByUserAsync(long userId); // Kullanıcıya ait siparişler
    }
}
