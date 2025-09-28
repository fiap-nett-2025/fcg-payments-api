using FCG.Payments.Application.DTO.Cart;
using FCG.Payments.Application.DTO.Order;
using FCG.Payments.Domain.Entities;

namespace FCG.Payments.Application.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(Guid userId);
        Task<Cart> AddItemAsync(Guid userId, Guid gameId, int quantity);
        Task<Cart> RemoveItemAsync(Guid userId, Guid gameId);
        Task ClearCartAsync(Guid userId);
        Task<OrderDto> CheckoutCartAsync(Guid userId);
    }
}
