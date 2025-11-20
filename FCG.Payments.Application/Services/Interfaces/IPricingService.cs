using FCG.Payments.Application.DTO.Game;
using FCG.Payments.Domain.Entities;

namespace FCG.Payments.Application.Services.Interfaces
{
    public interface IPricingService
    {
        Task<(decimal finalPrice, bool isPromotionalPrice)> CalculateFinalPrice(User user, GameDto game);
    }
}
