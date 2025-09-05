using Application.Dtos.Order;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Hafta 1: Sepetsiz stub sipariş oluştur
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateStubOrder([FromBody] CreateOrderDto request)
        {
            // TODO: Auth / userId gerçekten alınacak. Şimdilik mock userId = 1
            int userId = 1;

            var order = await _orderService.CreateStubOrderAsync(request, userId);

            return Ok(order);
        }

        /// <summary>
        /// Hafta 1: Stub sipariş getir
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound(new { message = "Sipariş bulunamadı." });

            return Ok(order);
        }
    }
}
