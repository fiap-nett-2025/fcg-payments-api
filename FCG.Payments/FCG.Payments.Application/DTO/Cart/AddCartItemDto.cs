namespace FCG.Payments.Application.DTO.Cart
{
    public class AddCartItemDto
    {
        public Guid GameId { get; set; }
        public int Quantity { get; set; }
    }
}
