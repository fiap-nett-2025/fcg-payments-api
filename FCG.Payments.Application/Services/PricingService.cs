using FCG.Payments.Application.DTO;
using FCG.Payments.Application.DTO.Game;
using FCG.Payments.Application.Extensions;
using FCG.Payments.Application.Services.Interfaces;
using FCG.Payments.Domain.Entities;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FCG.Payments.Application.Services
{
    public class PricingService(IHttpClientFactory httpClientFactory) : IPricingService
    {
        public async Task<(decimal finalPrice, bool isPromotionalPrice)> CalculateFinalPrice(User user, GameDto game)
        {
            IEnumerable<PromotionDto> activePromotions = await GetActivePromotions(user);

            var applicablePromotions = activePromotions
                .Where(p => p.IsActive && game.Genres.Contains(p.TargetGenre));

            if (!applicablePromotions.Any())
                return (game.Price, isPromotionalPrice: false);

            var finalPrice = applicablePromotions
                .Select(p => p.CalculateDiscountedPrice(game.Genres, game.Price))
                .Min();

            return (finalPrice, isPromotionalPrice: finalPrice != game.Price);
        }

        private async Task<IEnumerable<PromotionDto>> GetActivePromotions(User user)
        {
            var client = httpClientFactory.CreateClient("GamesApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            var response = await client.GetAsync($"api/Promotion/Actived");
            if (!response.IsSuccessStatusCode)
                return [];

            var responseDto = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<PromotionDto>>>();
            return responseDto?.Data ?? [];
        }
    }
}
