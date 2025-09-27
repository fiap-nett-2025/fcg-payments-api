namespace FCG.Payments.Application.DTO.Cart
{
    public class CartDto
    {
        public List<CartItemDto> Items { get; set; } = [];
        public decimal Total { get; set; }
    }
}
