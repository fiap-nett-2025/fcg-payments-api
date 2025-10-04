using FCG.Payments.Domain.Enums;
using System.Text.Json.Serialization;

namespace FCG.Payments.Application.DTO.Payment
{
    public class PaymentGatewayResponseDto
    {
        public string? UserId { get; set; }
        public string? OrderId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime PaymentStatusDate { get; set; }
    }
}
