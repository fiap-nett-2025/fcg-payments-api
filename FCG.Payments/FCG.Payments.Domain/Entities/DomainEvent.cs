using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Payments.Domain.Entities
{
    public abstract class DomainEvent
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
        public string EventType => GetType().Name;
    }
}
