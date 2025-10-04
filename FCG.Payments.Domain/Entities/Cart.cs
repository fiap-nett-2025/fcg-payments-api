using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Payments.Domain.Entities
{
    public class Cart(Guid userId)
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)] 
        public Guid Id { get; private set; } = Guid.NewGuid();

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid UserId { get; private set; } = userId;
        public List<CartItem> Items { get; private set; } = [];

        public void AddItem(string gameId, decimal price)
        {
            var item = Items.FirstOrDefault(i => i.GameId == gameId);
            if (item == null) Items.Add(new CartItem(gameId, price));
        }

        public void RemoveItem(string gameId)
        {
            var item = Items.FirstOrDefault(i => i.GameId == gameId);
            if (item != null) Items.Remove(item);
        }

        public void Clear() => Items.Clear();
        public decimal GetTotal() => Items.Sum(i => i.UnitPrice);
    }
}
