using FCG.Payments.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FCG.Payments.Application.Services
{
    public class PaymentGatewayService(ILogger<UserService> logger, IHttpClientFactory httpClientFactory) : IPaymentGatewayService
    {
        public Task<bool> SendPaymentRequest()
        {
            //TODO: Conectar com Azure Function
            //var client = httpClientFactory.CreateClient("PaymentGateway");
            //try
            //{
            //    var response = await client.PostAsync($"api/", null);
            //    response.EnsureSuccessStatusCode();
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    logger.LogError(ex, "Error sending payment for order {OrderId}",);
            //    return false;
            //}
            return Task.FromResult(true);
        }
    }
}
