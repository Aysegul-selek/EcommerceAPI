using Application.Dtos.Order;
using Application.Dtos.ResponseDto;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class OrderManager : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderManager(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponseDto<OrderDto>> CreateStubOrderAsync(CreateOrderDto request, long userId, string? idempotencyKey = null)
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

           
            var dto = _mapper.Map<OrderDto>(order);

            return new ApiResponseDto<OrderDto>
            {
                Success = true,
                Message = "Sipariş başarıyla oluşturuldu",
                Data = dto
            };
        }

        public async Task<ApiResponseDto<OrderDto?>> GetByIdAsync(long id)
        {
            var order = await _orderRepository.FindByIdAsync(id);
            if (order == null)
            {
                return new ApiResponseDto<OrderDto?>
                {
                    Success = false,
                    Message = "Sipariş bulunamadı",
                    Data = null,
                    ErrorCodes = "ORDER_NOT_FOUND"
                };
            }

            var dto = _mapper.Map<OrderDto>(order);

            return new ApiResponseDto<OrderDto?>
            {
                Success = true,
                Message = "Sipariş getirildi",
                Data = dto
            };
        }
    }
}
