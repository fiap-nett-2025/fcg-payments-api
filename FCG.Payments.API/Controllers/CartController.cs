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
            var user = GetUserFromRequest();
            var cart = await cartService.GetCartAsync(user);

            return Success(cart, "Carrinho encontrado/criado com sucesso.");
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddItem([FromBody] AddCartItemDto dto)
        {
            var user = GetUserFromRequest();
            var cart = await cartService.AddItemAsync(user, dto.GameId);
            return CreatedResponse(cart, "Item adicionado com sucesso.");
        }

        [HttpDelete("remove/{gameId}")]
        public async Task<IActionResult> RemoveItem(string gameId)
        {
            var user = GetUserFromRequest();
            var cart = await cartService.RemoveItemAsync(user, gameId);
            return Success(cart, "Item removido com sucesso.");
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var user = GetUserFromRequest();
            await cartService.ClearCartAsync(user);
            return NoContent();
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()
        {
            var user = GetUserFromRequest();
            var order = await cartService.CheckoutCartAsync(user);
            return Success(order, "Ordem de pagamento criada com sucesso.");
        }
    }
}
