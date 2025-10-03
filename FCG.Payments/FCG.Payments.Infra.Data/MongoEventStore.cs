using FCG.Payments.Domain.Events;
using FCG.Payments.Infra.Data.Repository.Interfaces;
using MongoDB.Driver;

namespace FCG.Payments.Infra.Data
{
    public class MongoEventStore(IMongoDatabase db) : IEventStore
    {
        private IMongoCollection<DomainEvent> _events = db.GetCollection<DomainEvent>("Events");

        public async Task SaveAsync(DomainEvent @event) 
            => await _events.InsertOneAsync(@event);

        public async Task<List<DomainEvent>> GetEventsAsync(Guid aggregateId) 
            => await _events.Find(_ => true).ToListAsync();
    }
}
