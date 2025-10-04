using FCG.Payments.Application.DTO.Game;
using FCG.Payments.Domain.Entities;

namespace FCG.Payments.Application.Services.Interfaces
{
    public interface IGameService
    {
        Task<GameDto?> GetGameByIdAsync(User user, string gameId);
        Task IncreaseGamesPopularity(User user, params string[] gamesId);
    }
}
