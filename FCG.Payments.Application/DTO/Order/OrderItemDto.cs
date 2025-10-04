namespace FCG.Payments.Application.DTO.Order
{
    public class OrderItemDto
    {
        public required string GameId { get; set; }
        public decimal Price { get; set; }
    }
}
