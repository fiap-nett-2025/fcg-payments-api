using FCG.Payments.Application.DTO;
using FCG.Payments.Application.DTO.Game;
using FCG.Payments.Application.Publishers.Interfaces;
using FCG.Payments.Application.Services.Interfaces;
using FCG.Payments.Domain.Entities;
using FCG.Payments.Domain.Enums;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FCG.Payments.Application.Services
{

    public class GameService(IHttpClientFactory httpClientFactory, IGameServicePublisher gameServicePublisher) : IGameService
    {
        public async Task<GameDto?> GetGameByIdAsync(User user, string gameId)
        {
            var client = httpClientFactory.CreateClient("GamesApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            var response = await client.GetAsync($"api/Game/{gameId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var responseDto = await response.Content.ReadFromJsonAsync<ApiResponse<GameDto>>();
            return responseDto?.Data;
        }

        public Task IncreaseGamesPopularity(User user, params string[] gamesId)
        {
            var dto = new IncreaseGamePopularityDTO
            {
                UserId = user.Id,
                GamesId = gamesId
            };
            return gameServicePublisher.IncreasePopularityAsync(dto);
        }
    }
}
