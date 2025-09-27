using FCG.Payments.Application.DTO.Cart;
using FCG.Payments.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Payments.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class CartController(ICartService cartService) : ControllerBase
    {
        private Guid GetUserId()
        {
            return Guid.Parse("1e348abb-f544-4242-8654-0b5caa33379a"); // Guid.Parse(User.FindFirst("sub")!.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();
            var cart = await cartService.GetCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddItem([FromBody] AddCartItemDto dto)
        {
            var userId = GetUserId();
            await cartService.AddItemAsync(userId, dto.GameId, dto.Quantity);
            return Ok();
        }

        [HttpDelete("remove/{gameId}")]
        public async Task<IActionResult> RemoveItem(Guid gameId)
        {
            var userId = GetUserId();
            await cartService.RemoveItemAsync(userId, gameId);
            return Ok();
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserId();
            await cartService.ClearCartAsync(userId);
            return Ok();
        }
    }
}
