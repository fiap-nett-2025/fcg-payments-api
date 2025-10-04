namespace FCG.Payments.Application.DTO.Cart
{
    public class CartDto
    {
        public List<CartItemDto> Items { get; set; } = [];
        public decimal Total { get; set; }

        public static CartDto ToDto(Domain.Entities.Cart cart)
        {
            return new CartDto()
            {
                Items = [.. cart.Items.Select(i => new CartItemDto()
                {
                    GameId = i.GameId,
                    UnitPrice = i.UnitPrice
                })],
                Total = cart.GetTotal()
            };
        }
    }
}
