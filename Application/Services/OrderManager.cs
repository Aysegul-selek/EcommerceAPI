using Application.Dtos.Order;
using Application.Dtos.ResponseDto;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
<<<<<<< HEAD
using Application.Pipelines.Order;
=======
>>>>>>> DevB-1
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class OrderManager : IOrderService
    {
        private readonly OrderFactory _orderFactory;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

<<<<<<< HEAD
        public OrderManager(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            OrderFactory orderFactory)
=======
        public OrderManager(IOrderRepository orderRepository,
                            IProductRepository productRepository,
                            IUnitOfWork unitOfWork,
                            IMapper mapper)
>>>>>>> DevB-1
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
<<<<<<< HEAD
            _orderFactory = orderFactory;
=======
>>>>>>> DevB-1
        }

        public async Task<ApiResponseDto<OrderDto>> CreateStubOrderAsync(CreateOrderDto request, long userId, string? idempotencyKey = null)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
<<<<<<< HEAD
                // Pipeline kullanarak Order entity oluştur
                var order = await _orderFactory.CreateAsync(request, async productId =>
                {
                    // ProductRepository üzerinden ürün bilgilerini al
                    var product = await _productRepository.GetByIdAsync(productId);
                    if (product == null || !product.IsActive)
                        throw new Exception($"Ürün bulunamadı veya aktif değil. ProductId: {productId}");

                    var itemQuantity = request.Items.First(i => i.ProductId == productId).Quantity;
                    if (product.Stok < itemQuantity)
                        throw new Exception($"Yetersiz stok. ProductId: {productId}");

                    // Stok düş
                    product.Stok -= itemQuantity;
                    await _productRepository.Update(product);

                    // Fiyatı döndür
                    return product.Price;
                });

                // Order bilgilerini set et
                order.UserId = userId;
                order.OrderNo = "ORD-" + DateTime.UtcNow.Ticks;
                order.Status = "Pending";
                order.CreatedDate = DateTime.UtcNow;

                // Order kaydet
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
=======
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

                    // stok düş
                    product.Stok -= item.Quantity;
                    await _productRepository.Update(product);

                    // sipariş kalemi ekle
                    var orderItem = new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price
                    };

                    total += orderItem.Quantity * orderItem.UnitPrice;
                    order.Items.Add(orderItem);
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
>>>>>>> DevB-1
                    Success = false,
                    Message = $"Sipariş oluşturulamadı: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponseDto<OrderDto?>> GetByIdAsync(long id)
        {
            var order = await _orderRepository.FindByIdAsync(id);
            if (order == null)
<<<<<<< HEAD
=======
            {
>>>>>>> DevB-1
                return new ApiResponseDto<OrderDto?>
                {
                    Success = false,
                    Message = "Sipariş bulunamadı",
                    ErrorCodes = "ORDER_NOT_FOUND"
                };
<<<<<<< HEAD

            var dto = _mapper.Map<OrderDto>(order);
=======
            }

            var dto = _mapper.Map<OrderDto>(order);

>>>>>>> DevB-1
            return new ApiResponseDto<OrderDto?>
            {
                Success = true,
                Message = "Sipariş getirildi",
                Data = dto
            };
        }

        public async Task<ApiResponseDto<List<OrderDto>>> GetOrdersAsync()
        {
<<<<<<< HEAD
            var orders = await _orderRepository.GetAllAsync();
=======
            var orders= await _orderRepository.GetAllAsync();
>>>>>>> DevB-1
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
<<<<<<< HEAD
            var dtos = _mapper.Map<List<OrderDto>>(orders);
=======
            var dtos=_mapper.Map<List<OrderDto>>(orders);
>>>>>>> DevB-1
            return new ApiResponseDto<List<OrderDto>>
            {
                Success = true,
                Message = "Kullanıcı siparişleri listelendi",
                Data = dtos
            };
        }
    }
}
