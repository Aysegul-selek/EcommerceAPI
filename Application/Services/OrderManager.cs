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
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDiscountService _discountService;

        public OrderManager(IOrderRepository orderRepository,
                            IProductRepository productRepository,
                            IUnitOfWork unitOfWork,
                            IMapper mapper,
                            IDiscountService discountService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _discountService = discountService;
        }

        public async Task<ApiResponseDto<OrderDto>> CreateStubOrderAsync(CreateOrderDto request, long userId, string? idempotencyKey = null)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var order = new Order
                {
                    OrderNo = "ORD-" + DateTime.UtcNow.Ticks,
                    Status = "Pending",
                    UserId = userId,
                    CreatedDate = DateTime.UtcNow
                };

                decimal total = 0m;

                foreach (var item in request.Items)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product == null || !product.IsActive)
                        throw new Exception($"Ürün bulunamadı veya aktif değil. ProductId: {item.ProductId}");

                    if (product.Stok < item.Quantity)
                        throw new Exception($"Yetersiz stok. ProductId: {item.ProductId}");

                    // Stok düş
                    product.Stok -= item.Quantity;
                    await _productRepository.Update(product);

                    // Sipariş kalemi ekle
                    var orderItem = new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price
                    };

                    total += orderItem.Quantity * orderItem.UnitPrice;
                    order.Items.Add(orderItem);
                }

                // Discount uygulama
                if (request.Discount != null)
                {
                    if (request.Discount.Type == "Percentage")
                    {
                        total -= total * (request.Discount.Amount / 100m);
                    }
                    else if (request.Discount.Type == "Fixed")
                    {
                        total -= request.Discount.Amount;
                    }

                    if (total < 0) total = 0; // Negatif olmasın
                }

                order.Total = total;

                await _orderRepository.AddOrderAsync(order);
                await _orderRepository.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                var dto = _mapper.Map<OrderDto>(order);

                return new ApiResponseDto<OrderDto>
                {
                    Success = true,
                    Message = "Sipariş başarıyla oluşturuldu",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponseDto<OrderDto>
                {
                    Success = false,
                    Message = $"Sipariş oluşturulamadı: {ex.Message}"
                };
            }
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

        public async Task<ApiResponseDto<List<OrderDto>>> GetOrdersAsync()
        {
            var orders= await _orderRepository.GetAllAsync();
            var dtos = _mapper.Map<List<OrderDto>>(orders);
            return new ApiResponseDto<List<OrderDto>>
            {
                Success = true,
                Message = "Siparişler listelendi",
                Data = dtos
            };
        }

        public async Task<ApiResponseDto<List<OrderDto>>> GetOrdersByUserAsync(long userId)
        {
            var orders = (await _orderRepository.GetAllAsync()).Where(o => o.UserId == userId).ToList();
            var dtos=_mapper.Map<List<OrderDto>>(orders);
            return new ApiResponseDto<List<OrderDto>>
            {
                Success = true,
                Message = "Kullanıcı siparişleri listelendi",
                Data = dtos
            };
        }
    }
}
