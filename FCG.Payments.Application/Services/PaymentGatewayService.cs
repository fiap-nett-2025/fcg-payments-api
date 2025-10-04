using FCG.Payments.Application.DTO.Payment;
using FCG.Payments.Application.Services.Interfaces;
using FCG.Payments.Domain.Entities;
using FCG.Payments.Domain.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FCG.Payments.Application.Services
{
    public class PaymentGatewayService(ILogger<UserService> logger, IHttpClientFactory httpClientFactory) : IPaymentGatewayService
    {
        public async Task<(bool, string)> SendPaymentRequest(User user, Guid orderId)
        {
            try
            {
                var client = httpClientFactory.CreateClient("PaymentGateway");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

                var dto = new PaymentGatewayRequestDto { OrderId = orderId.ToString() };
                var json = JsonConvert.SerializeObject(dto);
                var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/Payment", httpContent);
                response.EnsureSuccessStatusCode();

                var teste = await response.Content.ReadAsStringAsync();

                var responseDto = await response.Content.ReadFromJsonAsync<PaymentGatewayResponseDto>();

                if (responseDto == null)
                    return (false, "");

                return (responseDto.PaymentStatus == PaymentStatus.Paid, responseDto.PaymentStatus.ToString());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending payment for order {OrderId}", orderId);
                return (false, "");
            }
        }
    }
}
