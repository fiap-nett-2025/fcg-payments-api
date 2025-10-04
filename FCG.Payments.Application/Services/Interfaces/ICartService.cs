using FCG.Payments.Application.DTO.Cart;
using FCG.Payments.Application.DTO.Order;
using FCG.Payments.Domain.Entities;

namespace FCG.Payments.Application.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(User user);
        Task<Cart> AddItemAsync(User user, Guid gameId, int quantity);
        Task<Cart> RemoveItemAsync(User user, Guid gameId);
        Task ClearCartAsync(User user);
        Task<OrderDto> CheckoutCartAsync(User user);
    }
}
