using FCG.Payments.Domain.Enums;

namespace FCG.Payments.Application.DTO.Game
{
    public class PromotionDto
    {
        public int DiscountPercentage { get; init; }
        public GameGenre TargetGenre { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
    }
}
