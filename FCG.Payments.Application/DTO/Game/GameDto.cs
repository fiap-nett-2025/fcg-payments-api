using FCG.Payments.Domain.Enums;
using Newtonsoft.Json;

namespace FCG.Payments.Application.DTO.Game
{
    public class GameDto
    {
        public required string Id { get; set; }
        public decimal Price { get; set; }
        public required GameGenre[] Genres { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
