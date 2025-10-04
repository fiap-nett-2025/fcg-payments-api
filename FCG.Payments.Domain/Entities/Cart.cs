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

        public void AddItem(Guid gameId, int qty, decimal price)
        {
            var item = Items.FirstOrDefault(i => i.GameId == gameId);
            if (item != null) item.IncreaseQuantity(qty);
            else Items.Add(new CartItem(gameId, qty, price));
        }

        public void RemoveItem(Guid gameId)
        {
            var item = Items.FirstOrDefault(i => i.GameId == gameId);
            if (item != null) Items.Remove(item);
        }

        public void Clear() => Items.Clear();
        public decimal GetTotal() => Items.Sum(i => i.Subtotal);
    }
}
