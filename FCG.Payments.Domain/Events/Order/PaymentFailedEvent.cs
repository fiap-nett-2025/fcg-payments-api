using FCG.Payments.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Payments.Domain.Events.Order
{
    public class PaymentFailedEvent : DomainEvent
    {
        public PaymentFailedEvent(Guid orderId)
        {
            OrderId = orderId;
            SetupAggregate<Entities.Order>(orderId.ToString());
        }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid OrderId { get; private set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid UserId { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public required string Reason { get; set; }
    }
}
