using FCG.Payments.Domain.Entities;
using FCG.Payments.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Payments.Domain.Events.Order
{
    public class OrderPaidEvent : DomainEvent
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid OrderId { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid UserId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
