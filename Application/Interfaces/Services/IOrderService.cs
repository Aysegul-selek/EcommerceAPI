using Application.Dtos.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateStubOrderAsync(CreateOrderDto request, long userId, string? idempotencyKey = null);
        Task<OrderDto?> GetByIdAsync(long id);
    }
}
