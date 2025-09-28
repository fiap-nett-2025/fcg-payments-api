using FCG.Payments.Application.DTO;
using FCG.Payments.Application.Services.Interfaces;
using System.Net.Http.Json;

namespace FCG.Payments.Application.Services
{
    public class GameService(IHttpClientFactory httpClientFactory) : IGameService
    {
        public async Task<GameDto?> GetGameByIdAsync(Guid gameId)
        {
            var client = httpClientFactory.CreateClient("GamesApi");

            var response = await client.GetAsync($"api/games/{gameId}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<GameDto>();
        }

        public async Task IncreaseGamePopularity(Guid gameId)
        {
            var client = httpClientFactory.CreateClient("GamesApi");
            var response = await client.PostAsync($"api/games/{gameId}/increase-popularity", null);
            response.EnsureSuccessStatusCode();
        }
    }
}
