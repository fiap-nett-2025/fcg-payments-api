using FCG.Payments.Domain.Events;

namespace FCG.Payments.Infra.Persistence.Repository.Interfaces
{
    public interface IEventStore
    {
        Task SaveAsync(DomainEvent @event);
        Task<List<DomainEvent>> GetEventsAsync(Guid aggregateId);
    }
}
