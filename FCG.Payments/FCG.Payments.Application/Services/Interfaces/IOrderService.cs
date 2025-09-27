using FCG.Payments.Application.DTO.Order;

namespace FCG.Payments.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderFromCartAsync(Guid userId);
        Task<OrderDto?> GetOrderAsync(Guid orderId);
        Task<PaymentResult> PayOrderAsync(Guid userId, Guid orderId, PayOrderDto dto);
    }
}
