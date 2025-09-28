namespace FCG.Payments.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task AddGamesInLibraryAsync(Guid userId, params Guid[] gamesId);
    }
}
