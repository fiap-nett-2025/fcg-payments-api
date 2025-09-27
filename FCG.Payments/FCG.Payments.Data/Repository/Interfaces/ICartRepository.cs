using FCG.Payments.Domain.Entities;

namespace FCG.Payments.Data.Repository.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(Guid userId);
        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
        Task DeleteAsync(Guid cartId);
    }
}
