using FCG.Payments.Domain.Enums;

namespace FCG.Payments.Domain.ValueObjects
{
    public class Discount(string code, DiscountType type, decimal value)
    {
        public string Code { get; private set; } = code;
        public DiscountType Type { get; private set; } = type;
        public decimal Value { get; private set; } = value;

        public decimal Apply(decimal total)
        {
            return Type switch
            {
                DiscountType.Percentage => total - (total * Value / 100),
                DiscountType.FixedAmount => total - Value,
                _ => total
            };
        }
    }
}
