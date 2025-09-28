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
        public async Task<CartDto> GetCartAsync(Guid userId)
        {
            logger.LogDebug("Retrieving cart for user {UserId}", userId);
            var cart = await cartRepository.GetByUserIdAsync(userId);

            if (cart is null)
            {
                cart = new Cart(userId);
                var taskInsert = cartRepository.AddAsync(cart);
                var taskEvent = eventStore.SaveAsync(new CartCreatedEvent
                {
                    CartId = cart.Id,
                    UserId = userId
                });

                await Task.WhenAll(taskInsert, taskEvent);
            }

            return CartDto.ToDto(cart);
        }

        public async Task<Cart> AddItemAsync(Guid userId, Guid gameId, int quantity)
        {
            if (quantity <= 0)
                throw new InvalidOperationException("Invalid quantity");

            var cart = await cartRepository.GetByUserIdAsync(userId)
                       ?? new Cart(userId);

            var game = await gameService.GetGameByIdAsync(gameId)
                ?? throw new InvalidOperationException("Game not found");

            cart.AddItem(gameId, quantity, game.Price);

            await cartRepository.UpdateAsync(cart);

            await eventStore.SaveAsync(new CartItemAddedEvent
            {
                CartId = cart.Id,
                GameId = gameId,
                Quantity = quantity,
                UnitPrice = game.Price
            });

            return cart;
        }

        public async Task<Cart> RemoveItemAsync(Guid userId, Guid gameId)
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

            return cart;
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

        public async Task<OrderDto> CheckoutCartAsync(Guid userId)
        {
            logger.LogDebug("Creating order from cart for user {UserId}", userId);
            var cart = await cartRepository.GetByUserIdAsync(userId)
                       ?? throw new InvalidOperationException("Cart not found");

            logger.LogDebug("Cart {CartId} from user {UserId} has {ItemCount} items", cart.Id, userId, cart.Items.Count);
            if (cart.Items.Count == 0)
                throw new InvalidOperationException("Cart is empty");

            var order = new Order(userId, cart.Items);

            var taskInsertOrder = orderRepository.AddAsync(order);
            var taskEvent = eventStore.SaveAsync(new OrderCreatedEvent
            {
                OrderId = order.Id,
                UserId = userId,
                TotalPrice = order.Total
            });

            await Task.WhenAll(taskInsertOrder, taskEvent);

            cart.Clear();
            await cartRepository.UpdateAsync(cart);
            return OrderDto.ToDto(order);
        }
    }
}
