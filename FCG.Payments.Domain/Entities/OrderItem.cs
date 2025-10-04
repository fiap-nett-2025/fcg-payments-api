using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Payments.Domain.Entities
{
    public class OrderItem(Guid gameId, int qty, decimal price)
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid GameId { get; private set; } = gameId;
        public int Quantity { get; private set; } = qty;
        public decimal UnitPrice { get; private set; } = price;
        public decimal Subtotal => Quantity * UnitPrice;
    }
}
