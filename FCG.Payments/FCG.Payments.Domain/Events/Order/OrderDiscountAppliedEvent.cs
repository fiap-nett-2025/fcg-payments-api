using FCG.Payments.Domain.Entities;
using FCG.Payments.Domain.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Payments.Domain.Events.Order
{
    public class OrderDiscountAppliedEvent : DomainEvent
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid OrderId { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid UserId { get; set; }
        public Discount? Discount { get; set; }
    }
}
