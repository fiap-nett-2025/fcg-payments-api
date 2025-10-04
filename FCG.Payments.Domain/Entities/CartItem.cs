using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Payments.Domain.Entities
{
    public class CartItem(string gameId, decimal unitPrice)
    {
        public string GameId { get; private set; } = gameId;
        public decimal UnitPrice { get; private set; } = unitPrice;
    }
}
