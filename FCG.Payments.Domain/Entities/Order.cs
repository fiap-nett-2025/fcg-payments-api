using FCG.Payments.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace FCG.Payments.Domain.Entities
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid UserId { get; private set; }
        public List<OrderItem> Items { get; private set; } = [];
        public decimal Total { get; private set; }
        public decimal Discount { get; private set; } = 0;
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;
        public bool IsPaid { get => this.Status == OrderStatus.Paid; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public Order(Guid userId, IEnumerable<CartItem> cartItems)
        {
            UserId = userId;
            foreach (var item in cartItems)
                Items.Add(new OrderItem(item.GameId, item.UnitPrice));
            RecalculateTotal();
        }

        private void RecalculateTotal()
        {
            Total = Items.Sum(i => i.UnitPrice) - Discount;
            if (Total < 0) Total = 0;
        }

        public void ApplyDiscount(decimal amount)
        {
            Discount = amount;
            RecalculateTotal();
        }

        public void MarkAsPaid() => Status = OrderStatus.Paid;
        public void Cancel() => Status = OrderStatus.Cancelled;
    }
}
