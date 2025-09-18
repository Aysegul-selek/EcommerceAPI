using Application.Dtos.Order;
using Application.Dtos.ResponseDto;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Pipelines.Order;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class OrderManager : IOrderService
    {
        private readonly OrderFactory _orderFactory;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IdempotencyService _idempotencyService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderManager(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            OrderFactory orderFactory,
            IdempotencyService idempotencyService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderFactory = orderFactory;
            _idempotencyService = idempotencyService;
        }

        public async Task<ApiResponseDto<OrderDto>> CreateStubOrderAsync(CreateOrderDto request, long userId, string? idempotencyKey = null)
        {
            if (!string.IsNullOrEmpty(idempotencyKey))
            {
                // Idempotency kontrolü
                var cached = await _idempotencyService.GetCachedResponseAsync<ApiResponseDto<OrderDto>>(idempotencyKey);
                if (cached != null)
                {
                    return cached; // Önceki response’u döndür
                }
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var order = await _orderFactory.CreateAsync(request, async productId =>
                {
                    var product = await _productRepository.GetByIdAsync(productId);
                    if (product == null || !product.IsActive)
                        throw new Exception($"Ürün bulunamadı veya aktif değil. ProductId: {productId}");

                    var itemQuantity = request.Items.First(i => i.ProductId == productId).Quantity;
                    if (product.Stok < itemQuantity)
                        throw new Exception($"Yetersiz stok. ProductId: {productId}");

                    product.Stok -= itemQuantity;
                    await _productRepository.Update(product);

                    return product.Price;
                });

                order.UserId = userId;
                order.OrderNo = "ORD-" + DateTime.UtcNow.Ticks;
                order.Status = "Pending";
                order.CreatedDate = DateTime.UtcNow;

                await _orderRepository.AddOrderAsync(order);
                await _orderRepository.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                var dto = _mapper.Map<OrderDto>(order);
                var response = new ApiResponseDto<OrderDto>
                {
                    Success = true,
                    Message = "Sipariş başarıyla oluşturuldu",
                    Data = dto
                };

                // Idempotency response kaydet
                if (!string.IsNullOrEmpty(idempotencyKey))
                    await _idempotencyService.SaveResponseAsync(idempotencyKey, response);

                return response;
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
                return new ApiResponseDto<OrderDto?>
                {
                    Success = false,
                    Message = "Sipariş bulunamadı",
                    ErrorCodes = "ORDER_NOT_FOUND"
                };

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
            var orders = await _orderRepository.GetAllAsync();
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
            var dtos = _mapper.Map<List<OrderDto>>(orders);
            return new ApiResponseDto<List<OrderDto>>
            {
                Success = true,
                Message = "Kullanıcı siparişleri listelendi",
                Data = dtos
            };
        }
    }
}
