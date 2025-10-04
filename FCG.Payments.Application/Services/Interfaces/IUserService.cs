using FCG.Payments.Domain.Entities;

namespace FCG.Payments.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task AddGamesInLibraryAsync(User user, params Guid[] gamesId);
    }
}
