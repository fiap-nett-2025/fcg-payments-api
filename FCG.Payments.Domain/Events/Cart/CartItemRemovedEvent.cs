using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Payments.Domain.Events.Cart
{
    public class CartItemRemovedEvent : DomainEvent
    {
        public CartItemRemovedEvent(Guid cartId)
        {
            CartId = cartId;
            SetupAggregate<Entities.Cart>(cartId.ToString());
        }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid CartId { get; private set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid UserId { get; set; }
        public required string GameId { get; set; }
    }
}
