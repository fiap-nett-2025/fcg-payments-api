using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Payments.Domain.Entities
{
    public class CartItem(Guid gameId, int quantity, decimal unitPrice)
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid GameId { get; private set; } = gameId;
        public int Quantity { get; private set; } = quantity;
        public decimal UnitPrice { get; private set; } = unitPrice;
        public decimal Subtotal => Quantity * UnitPrice;

        public void IncreaseQuantity(int qty) => Quantity += qty;
    }
}
