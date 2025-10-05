using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Payments.Domain.Events.Order
{
    public class OrderPaidEvent : DomainEvent
    {
        public OrderPaidEvent(Guid orderId)
        {
            OrderId = orderId;
            SetupAggregate<Entities.Order>(orderId.ToString());
        }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid OrderId { get; private set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid UserId { get; set; }

        public string? PaymentMethod { get; set; }
    }
}
