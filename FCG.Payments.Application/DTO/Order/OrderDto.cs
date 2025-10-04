using FCG.Payments.Domain.Enums;

namespace FCG.Payments.Application.DTO.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Total { get; set; }
        public List<OrderItemDto> Items { get; set; } = [];

        public static OrderDto ToDto(Domain.Entities.Order order)
        {
            var dto = new OrderDto()
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(i => new OrderItemDto()
                {
                    GameId = i.GameId,
                    Price = i.UnitPrice

                }).ToList(),
                Status = order.Status,
                Total = order.Total,
            };
            return dto;
        }
    }
}
