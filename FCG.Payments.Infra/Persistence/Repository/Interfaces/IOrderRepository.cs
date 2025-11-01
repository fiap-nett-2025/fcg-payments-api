using FCG.Payments.Domain.Entities;

namespace FCG.Payments.Infra.Persistence.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
    }
}
