using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Payments.Domain.Entities
{
    public class OrderItem(string gameId, decimal price)
    {
        public string GameId { get; private set; } = gameId;
        public decimal UnitPrice { get; private set; } = price;
    }
}
