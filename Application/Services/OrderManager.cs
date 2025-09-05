using Application.Dtos.Order;
using Application.Dtos.OrderItem;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class OrderManager : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderManager(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDto> CreateStubOrderAsync(CreateOrderDto request, long userId, string? idempotencyKey = null)
        {
            var order = new Order
            {
                OrderNo = "MOCK-" + DateTime.UtcNow.Ticks,
                Status = "Draft",
                Total = request.Items.Sum(i => i.Quantity * 10m), // fake fiyat
                UserId = userId,
                CreatedDate = DateTime.UtcNow
            };

            foreach (var item in request.Items)
            {
                order.Items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = 10m
                });
            }

            await _orderRepository.AddOrderAsync(order);
            await _orderRepository.SaveChangesAsync();

            return new OrderDto
            {
                Id = order.Id,
                OrderNo = order.OrderNo,
                TotalAmount = order.Total,
                Status = order.Status,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    SKU = "", // Swagger ile manuel doldurabilirsiniz
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity
                }).ToList()
            };
        }

        // Stub get by Id
        public async Task<OrderDto?> GetByIdAsync(long id)
        {
            var order = await _orderRepository.FindByIdAsync(id);
            if (order == null) return null;

            return new OrderDto
            {
                Id = order.Id,
                OrderNo = order.OrderNo,
                TotalAmount = order.Total,
                Status = order.Status,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    SKU = "", // Swagger ile manuel doldurabilirsiniz
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity
                }).ToList()
            };
        }
    }

}
