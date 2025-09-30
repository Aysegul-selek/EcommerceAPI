using Application.Dtos.Order;
using Application.Dtos.Pagination;
using Application.Dtos.ResponseDto;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IOrderService
    {
      
        Task<ApiResponseDto<OrderDto>> CreateStubOrderAsync(CreateOrderDto request, long userId, string? idempotencyKey = null);
        Task<ApiResponseDto<OrderDto?>> GetByIdAsync(long id);
        Task<ApiResponseDto<PagedResponse<OrderDto>>> GetOrdersAsync(PaginationFilter filter);        // Tüm siparişler
        Task<ApiResponseDto<List<OrderDto>>> GetOrdersByUserAsync(long userId); // Kullanıcıya ait siparişler
    }
}
