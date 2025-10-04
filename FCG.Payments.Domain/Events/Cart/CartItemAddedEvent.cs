using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Payments.Domain.Events.Cart
{
    public class CartItemAddedEvent : DomainEvent
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid CartId { get; set; }
        public required string GameId { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
