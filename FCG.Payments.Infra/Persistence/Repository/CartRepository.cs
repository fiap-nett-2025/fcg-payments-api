using FCG.Payments.Domain.Entities;
using FCG.Payments.Infra.Persistence.Repository.Interfaces;
using MongoDB.Driver;

namespace FCG.Payments.Infra.Persistence.Repository
{
    public class CartRepository(IMongoDatabase database) : ICartRepository
    {
        private IMongoCollection<Cart> _carts = database.GetCollection<Cart>("Carts");

        public async Task<Cart?> GetByUserIdAsync(Guid userId)
        {
            return await _carts.Find(c => c.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Cart cart)
        {
            await _carts.InsertOneAsync(cart);
        }

        public async Task UpdateAsync(Cart cart)
        {
            await _carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
        }

        public async Task DeleteAsync(Guid cartId)
        {
            await _carts.DeleteOneAsync(c => c.Id == cartId);
        }
    }
}
