using FCG.Payments.Domain.Enums;

namespace FCG.Payments.Application.DTO.Game
{
    public class GameDto
    {
        public required string Id { get; set; }
        public decimal Price { get; set; }
        public required GameGenre[] Genres { get; set; }
    }
}
