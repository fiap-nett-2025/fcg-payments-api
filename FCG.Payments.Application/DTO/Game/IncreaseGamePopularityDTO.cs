namespace FCG.Payments.Application.DTO.Game
{
    public class IncreaseGamePopularityDTO
    {
        public required Guid UserId { get; init; }
        public required string[] GamesId { get; init; }
    }
}
