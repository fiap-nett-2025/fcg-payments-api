using FCG.Payments.Application.DTO.Order;
using FCG.Payments.Domain.Entities;

namespace FCG.Payments.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto?> GetOrderAsync(Guid orderId);
        Task<PaymentResult> PayOrderAsync(User user, Guid orderId, PayOrderDto dto);
    }
}
