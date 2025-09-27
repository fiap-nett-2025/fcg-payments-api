using FCG.Payments.Application.DTO.Order;
using FCG.Payments.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Payments.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController(IOrderService orderService) : ControllerBase
    {
        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirst("sub")!.Value);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrderFromCart()
        {
            var userId = GetUserId();
            var order = await orderService.CreateOrderFromCartAsync(userId);
            return Ok(order);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            var order = await orderService.GetOrderAsync(orderId);
            if (order == null) return NotFound();
            return Ok(order);
        }


        [HttpPost("{orderId}/pay")]
        public async Task<IActionResult> PayOrder(Guid orderId, [FromBody] PayOrderDto dto)
        {
            var userId = GetUserId();
            var result = await orderService.PayOrderAsync(userId, orderId, dto);

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
