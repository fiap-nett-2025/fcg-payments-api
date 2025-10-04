namespace FCG.Payments.Application.DTO.Cart
{
    public class CartItemDto
    {
        public Guid GameId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get => Quantity * UnitPrice; }
    }
}
