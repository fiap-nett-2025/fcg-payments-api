using FCG.Payments.Application.DTO.Game;
using FCG.Payments.Domain.Enums;

namespace FCG.Payments.Application.Extensions
{
    public static class PromotionExtensions
    {
        public static bool IsActive(this PromotionDto dto)
            =>  DateTime.UtcNow >= dto.StartDate && DateTime.UtcNow <= dto.EndDate;

        public static decimal CalculateDiscountedPrice(this PromotionDto dto, GameGenre[] genres, decimal price)
        {
            if (!dto.IsActive()) return price;
            if (!genres.Contains(dto.TargetGenre)) return price;
            return price * (1 - dto.DiscountPercentage / 100m);
        }
    }
}
