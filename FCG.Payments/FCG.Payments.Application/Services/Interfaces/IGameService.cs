using FCG.Payments.Application.DTO;
using FCG.Payments.Domain.Entities;

namespace FCG.Payments.Application.Services.Interfaces
{
    public interface IGameService
    {
        Task<GameDto?> GetGameByIdAsync(User user, Guid gameId);
        Task IncreaseGamesPopularity(User user, params Guid[] gamesId);
    }
}
