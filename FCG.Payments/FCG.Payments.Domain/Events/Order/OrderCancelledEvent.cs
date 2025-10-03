using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Payments.Domain.Events.Order
{
    public class OrderCancelledEvent : DomainEvent
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid OrderId { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid UserId { get; set; }
    }
}
