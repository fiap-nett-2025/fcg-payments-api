using FCG.Payments.Application.DTO.Cart;
using FCG.Payments.Application.DTO.Order;
using FCG.Payments.Application.Services.Interfaces;
using FCG.Payments.Domain.Entities;
using FCG.Payments.Domain.Events.Cart;
using FCG.Payments.Domain.Events.Order;
using FCG.Payments.Infra.Data.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace FCG.Payments.Application.Services
{
    public class CartService(
        ILogger<CartService> logger,
        ICartRepository cartRepository,
        IOrderRepository orderRepository,
        IEventStore eventStore,
        IGameService gameService
    ) : ICartService
    {
        public async Task<CartDto> GetCartAsync(User user)
        {
            logger.LogDebug("Retrieving cart for user {UserId}", user.Id);
            var cart = await cartRepository.GetByUserIdAsync(user.Id);

            if (cart is null)
            {
                cart = new Cart(user.Id);
                var taskInsert = cartRepository.AddAsync(cart);
                var taskEvent = eventStore.SaveAsync(new CartCreatedEvent
                {
                    CartId = cart.Id,
                    UserId = user.Id
                });

                await Task.WhenAll(taskInsert, taskEvent);
            }

            return CartDto.ToDto(cart);
        }

        public async Task<Cart> AddItemAsync(User user, string gameId)
        {
            var cart = await cartRepository.GetByUserIdAsync(user.Id)
                       ?? new Cart(user.Id);

            var game = await gameService.GetGameByIdAsync(user, gameId)
                ?? throw new InvalidOperationException("Game not found");

            cart.AddItem(gameId, game.Price);

            await cartRepository.UpdateAsync(cart);

            await eventStore.SaveAsync(new CartItemAddedEvent
            {
                CartId = cart.Id,
                GameId = gameId,
                UnitPrice = game.Price
            });

            return cart;
        }

        public async Task<Cart> RemoveItemAsync(User user, string gameId)
        {
            var cart = await cartRepository.GetByUserIdAsync(user.Id)
                       ?? throw new InvalidOperationException("Cart not found");

           if(cart.Items.Exists(i => i.GameId == gameId) == false)
                throw new InvalidOperationException("Item not found in cart");

            cart.RemoveItem(gameId);

            await cartRepository.UpdateAsync(cart);

            await eventStore.SaveAsync(new CartItemRemovedEvent
            {
                CartId = cart.Id,
                GameId = gameId
            });

            return cart;
        }

        public async Task ClearCartAsync(User user)
        {
            var cart = await cartRepository.GetByUserIdAsync(user.Id)
                       ?? throw new InvalidOperationException("Cart not found");

            cart.Clear();

            await cartRepository.UpdateAsync(cart);

            await eventStore.SaveAsync(new CartClearedEvent
            {
                CartId = cart.Id
            });
        }

        public async Task<OrderDto> CheckoutCartAsync(User user)
        {
            logger.LogDebug("Creating order from cart for user {UserId}", user.Id);
            var cart = await cartRepository.GetByUserIdAsync(user.Id)
                       ?? throw new InvalidOperationException("Cart not found");

            logger.LogDebug("Cart {CartId} from user {UserId} has {ItemCount} items", cart.Id, user.Id, cart.Items.Count);
            if (cart.Items.Count == 0)
                throw new InvalidOperationException("Cart is empty");

            var order = new Order(user.Id, cart.Items);

            var taskInsertOrder = orderRepository.AddAsync(order);
            var taskEvent = eventStore.SaveAsync(new OrderCreatedEvent
            {
                OrderId = order.Id,
                UserId = user.Id,
                TotalPrice = order.Total
            });

            await Task.WhenAll(taskInsertOrder, taskEvent);

            cart.Clear();
            await cartRepository.UpdateAsync(cart);
            return OrderDto.ToDto(order);
        }
    }
}
