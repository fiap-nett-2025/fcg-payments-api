using FCG.Payments.Domain.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Payments.Domain.Events.Cart
{
    public class CartItemAddedEvent : DomainEvent
    {
        public CartItemAddedEvent(Guid cartId)
        {
            CartId = cartId;
            SetupAggregate<Entities.Cart>(cartId.ToString());
        }


        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid CartId { get; private set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid UserId { get; set; }

        public required string GameId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal OriginalPrice { get; set; }
        public bool IsPromotionalPrice { get; set; }
    }
}
