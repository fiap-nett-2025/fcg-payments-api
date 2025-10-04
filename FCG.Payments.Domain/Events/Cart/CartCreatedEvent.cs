using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Payments.Domain.Events.Cart
{
    public class CartCreatedEvent : DomainEvent
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid CartId { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid UserId { get; set; }
    }
}
