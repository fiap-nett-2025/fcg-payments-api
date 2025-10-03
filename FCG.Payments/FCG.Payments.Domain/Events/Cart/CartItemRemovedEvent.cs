using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Payments.Domain.Events.Cart
{
    public class CartItemRemovedEvent : DomainEvent
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid CartId { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid GameId { get; set; }
    }
}
