using FCG.Payments.Application.DTO.Cart;
using FCG.Payments.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Payments.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController(ICartService cartService) : ApiBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();
            var cart = await cartService.GetCartAsync(userId);

            return Success(cart, "Carrinho encontrado/criado com sucesso.");
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddItem([FromBody] AddCartItemDto dto)
        {
            var userId = GetUserId();
            var cart = await cartService.AddItemAsync(userId, dto.GameId, dto.Quantity);
            return CreatedResponse(cart, "Item adicionado com sucesso.");
        }

        [HttpDelete("remove/{gameId}")]
        public async Task<IActionResult> RemoveItem(Guid gameId)
        {
            var userId = GetUserId();
            var cart = await cartService.RemoveItemAsync(userId, gameId);
            return Success(cart, "Item removido com sucesso.");
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserId();
            await cartService.ClearCartAsync(userId);
            return NoContent();
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()
        {
            var userId = GetUserId();
            var order = await cartService.CheckoutCartAsync(userId);
            return Success(order, "Ordem de pagamento criada com sucesso.");
        }
    }
}
