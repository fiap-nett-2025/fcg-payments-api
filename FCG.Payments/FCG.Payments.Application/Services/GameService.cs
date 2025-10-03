using FCG.Payments.Application.DTO;
using FCG.Payments.Application.Services.Interfaces;
using FCG.Payments.Domain.Entities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FCG.Payments.Application.Services
{
    public class GameService(IHttpClientFactory httpClientFactory) : IGameService
    {
        public async Task<GameDto?> GetGameByIdAsync(User user, Guid gameId)
        {
            var client = httpClientFactory.CreateClient("GamesApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            var response = await client.GetAsync($"api/Game/{gameId}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<GameDto>();
        }

        public async Task IncreaseGamesPopularity(User user, params Guid[] gamesId)
        {
            var client = httpClientFactory.CreateClient("GamesApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            var json = JsonConvert.SerializeObject(gamesId);
            var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PatchAsync($"api/Game/Popularity", httpContent);

            response.EnsureSuccessStatusCode();
        }
    }
}
