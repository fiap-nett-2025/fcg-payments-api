using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Payments.Domain.Events.Order
{
    public class OrderCreatedEvent : DomainEvent
    {
        public OrderCreatedEvent(Guid orderId)
        {
            OrderId = orderId;
            SetupAggregate<Entities.Order>(orderId.ToString());
        }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid OrderId { get; private set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid UserId { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
