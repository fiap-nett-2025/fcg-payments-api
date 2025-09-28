using FCG.Payments.Domain.Enums;

namespace FCG.Payments.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public PaymentMethod Method { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime PaidAt { get; private set; }
        public PaymentStatus Status { get; private set; }

        public Payment(Guid orderId, PaymentMethod method, decimal amount)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            Method = method;
            Amount = amount;
            PaidAt = DateTime.UtcNow;
            Status = PaymentStatus.Success;
        }
    }
}
