using FCG.Payments.Application.DTO;
using FCG.Payments.Application.DTO.Cart;
using FCG.Payments.Application.Services.Interfaces;
using FCG.Payments.Data.Repository.Interfaces;
using FCG.Payments.Domain.Entities;
using FCG.Payments.Domain.Events.Cart;

namespace FCG.Payments.Application.Services
{
    public class CartService(ICartRepository cartRepository, IEventStore eventStore) : ICartService
    {
        public async Task<CartDto> GetCartAsync(Guid userId)
        {
            var cart = await cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart(userId);
                await cartRepository.AddAsync(cart);

                await eventStore.SaveAsync(new CartCreatedEvent
                {
                    CartId = cart.Id,
                    UserId = userId
                });
            }

            return ToDto(cart);
        }

        public async Task AddItemAsync(Guid userId, Guid gameId, int quantity)
        {
            var cart = await cartRepository.GetByUserIdAsync(userId)
                       ?? new Cart(userId);

            // TODO: GET API DE GAMES
            var game = new GameDto()
            {
                Id = gameId,
                Title = "Sample Game",
                Price = 59.99m
            };

            cart.AddItem(gameId, quantity, game.Price);

            await cartRepository.UpdateAsync(cart);

            await eventStore.SaveAsync(new CartItemAddedEvent
            {
                CartId = cart.Id,
                GameId = gameId,
                Quantity = quantity,
                UnitPrice = game.Price
            });
        }

        public async Task RemoveItemAsync(Guid userId, Guid gameId)
        {
            var cart = await cartRepository.GetByUserIdAsync(userId)
                       ?? throw new InvalidOperationException("Cart not found");

            cart.RemoveItem(gameId);

            await cartRepository.UpdateAsync(cart);

            await eventStore.SaveAsync(new CartItemRemovedEvent
            {
                CartId = cart.Id,
                GameId = gameId
            });
        }

        public async Task ClearCartAsync(Guid userId)
        {
            var cart = await cartRepository.GetByUserIdAsync(userId)
                       ?? throw new InvalidOperationException("Cart not found");

            cart.Clear();

            await cartRepository.UpdateAsync(cart);

            await eventStore.SaveAsync(new CartClearedEvent
            {
                CartId = cart.Id
            });
        }

        private static CartDto ToDto(Cart cart)
        {
            return new CartDto()
            {
                Items = [.. cart.Items.Select(i => new CartItemDto()
                {
                    GameId = i.GameId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                })],
                Total = cart.GetTotal()
            };
        }
    }
}
