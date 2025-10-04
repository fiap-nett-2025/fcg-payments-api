namespace FCG.Payments.Application.DTO.Cart
{
    public class CartItemDto
    {
        public required string GameId { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
