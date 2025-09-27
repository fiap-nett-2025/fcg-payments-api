using FCG.Payments.Domain.Enums;
using FCG.Payments.Domain.ValueObjects;

namespace FCG.Payments.Application.DTO.Order
{
    public record PayOrderDto(PaymentMethod Method, Discount? Discount);
}
