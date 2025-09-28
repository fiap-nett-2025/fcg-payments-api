using FCG.Payments.Application.DTO;

namespace FCG.Payments.Application.Services.Interfaces
{
    public interface IGameService
    {
        Task<GameDto?> GetGameByIdAsync(Guid gameId);
        Task IncreaseGamePopularity(Guid gameId);
    }
}
