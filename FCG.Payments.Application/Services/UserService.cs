using FCG.Payments.Application.Services.Interfaces;
using FCG.Payments.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace FCG.Payments.Application.Services
{
    public class UserService(ILogger<UserService> logger, IHttpClientFactory httpClientFactory) : IUserService
    {
        public async Task AddGamesInLibraryAsync(User user, params Guid[] gamesId)
        {
            var client = httpClientFactory.CreateClient("UsersApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            foreach (Guid gameId in gamesId)
            {
                try
                {
                    var response = await client.PostAsync($"api/UserGameLibrary/{user.Id}/game/{gameId}", null);
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error adding game {GameId} to user {UserId} library", gameId, user.Id);
                }
            }
        }
    }
}
