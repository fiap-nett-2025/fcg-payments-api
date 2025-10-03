using FCG.Payments.Domain.Entities;

namespace FCG.Payments.Application.Services.Interfaces
{
    public interface IPaymentGatewayService
    {
        Task<bool> SendPaymentRequest(User user, Guid orderId);
    }
}
