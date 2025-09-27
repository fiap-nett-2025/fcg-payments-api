using FCG.Payments.Domain.Entities;

namespace FCG.Payments.Data.Repository.Interfaces
{
    public interface IEventStore
    {
        Task SaveAsync(DomainEvent @event);
        Task<List<DomainEvent>> GetEventsAsync(Guid aggregateId);
    }
}
