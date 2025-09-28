using FCG.Payments.Domain.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Payments.Domain.Events.Cart
{
    public class CartItemAddedEvent : DomainEvent
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid CartId { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid GameId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
