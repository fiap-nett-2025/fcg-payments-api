using FCG.Payments.Application.DTO;
using FCG.Payments.Application.Services.Interfaces;
using FCG.Payments.Domain.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FCG.Payments.Application.Services
{
    public class PaymentGatewayService(ILogger<UserService> logger, IHttpClientFactory httpClientFactory) : IPaymentGatewayService
    {
        public async Task<bool> SendPaymentRequest(User user, Guid orderId)
        {
            try
            {
                var client = httpClientFactory.CreateClient("PaymentGateway");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

                var dto = new PaymentGatewayDto { OrderId = orderId.ToString() };
                var json = JsonConvert.SerializeObject(dto);
                var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/Payment", httpContent);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending payment for order {OrderId}", orderId);
                return false;
            }
        }
    }
}
