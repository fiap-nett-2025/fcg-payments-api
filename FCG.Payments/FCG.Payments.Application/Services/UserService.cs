using FCG.Payments.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FCG.Payments.Application.Services
{
    public class UserService(ILogger<UserService> logger, IHttpClientFactory httpClientFactory) : IUserService
    {
        public async Task AddGamesInLibraryAsync(Guid userId, params Guid[] gamesId)
        {
            var client = httpClientFactory.CreateClient("UsersApi");
            foreach (Guid gameId in gamesId)
            {
                try
                {
                    var response = await client.PostAsync($"api/UserGameLibrary/{userId}/game/{gameId}", null);
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error adding game {GameId} to user {UserId} library", gameId, userId);
                }
            }
        }
    }
}
