using FCG.Payments.Application.DTO.Cart;

namespace FCG.Payments.Application.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(Guid userId);
        Task AddItemAsync(Guid userId, Guid gameId, int quantity);
        Task RemoveItemAsync(Guid userId, Guid gameId);
        Task ClearCartAsync(Guid userId);
    }
}
