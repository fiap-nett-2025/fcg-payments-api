using FCG.Payments.Domain.Entities;
using FCG.Payments.Infra.Persistence.Repository.Interfaces;
using MongoDB.Driver;

namespace FCG.Payments.Infra.Persistence.Repository
{
    public class OrderRepository(IMongoDatabase database) : IOrderRepository
    {
        private IMongoCollection<Order> _orders = database.GetCollection<Order>("Orders");

        public async Task<Order?> GetByIdAsync(Guid id) 
            => await _orders.Find(o => o.Id == id).FirstOrDefaultAsync();

        public async Task<List<Order>> GetByUserIdAsync(Guid userId) 
            => await _orders.Find(o => o.UserId == userId).ToListAsync();

        public async Task AddAsync(Order order) 
            => await _orders.InsertOneAsync(order);

        public async Task UpdateAsync(Order order) 
            => await _orders.ReplaceOneAsync(o => o.Id == order.Id, order);
    }
}
