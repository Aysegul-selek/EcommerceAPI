using Application.Dtos.Order;
using Application.Dtos.Pagination;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
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
        /// Sepetsiz stub sipariş oluştur
        /// </summary>
        [HttpPost("create/{userId}")]
        public async Task<ActionResult> CreateStubOrder(long userId, [FromBody] CreateOrderDto request)
        {
            var idempotencyKey = Request.Headers["Idempotency-Key"].FirstOrDefault();

            var result = await _orderService.CreateStubOrderAsync(request, userId, idempotencyKey);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        /// <summary>
        /// Sipariş detayını getir
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var result = await _orderService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Tüm siparişleri listele
        /// </summary>
        [HttpGet("list")]
        public async Task<IActionResult> GetOrders([FromQuery] PaginationFilter filter)
        {
            var result = await _orderService.GetOrdersAsync(filter);
            return Ok(result);
        }

        /// <summary>
        /// Kullanıcıya ait siparişleri listele
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetOrdersByUser(long userId)
        {
            var result = await _orderService.GetOrdersByUserAsync(userId);
            return Ok(result);
        }
    }
}
