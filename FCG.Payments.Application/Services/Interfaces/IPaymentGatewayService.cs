using FCG.Payments.Domain.Entities;

namespace FCG.Payments.Application.Services.Interfaces
{
    public interface IPaymentGatewayService
    {
        Task<(bool, string)> SendPaymentRequest(User user, Guid orderId);
    }
}
