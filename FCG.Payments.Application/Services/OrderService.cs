using FCG.Payments.Application.DTO.Order;
using FCG.Payments.Application.Services.Interfaces;
using FCG.Payments.Domain.Entities;
using FCG.Payments.Domain.Events.Order;
using FCG.Payments.Infra.Persistence.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace FCG.Payments.Application.Services
{
    public class OrderService(
        ILogger<OrderService> logger,
        IOrderRepository orderRepository,
        IEventStore eventStore,
        IUserService userService,
        IGameService gameService,
        IPaymentGatewayService paymentGateway
    ) : IOrderService
    {
        public async Task<OrderDto?> GetOrderAsync(Guid orderId)
        {
            var order = await orderRepository.GetByIdAsync(orderId);
            return order == null ? null : OrderDto.ToDto(order);
        }

        public async Task<PaymentResult> PayOrderAsync(User user, Guid orderId, PayOrderDto dto)
        {
            logger.LogDebug("Processing payment for order {OrderId} by user {UserId}", orderId, user.Id);
            var order = await orderRepository.GetByIdAsync(orderId)
                       ?? throw new InvalidOperationException("Ordem de pagamento não encontrada.");

            var gameIds = order.Items.Select(i => i.GameId).ToArray();
            logger.LogDebug("Order {OrderId} has {ItemCount} games: {GamesIds}", orderId, order.Items.Count, string.Join(", ", gameIds));

            if (order.UserId != user.Id)
                throw new UnauthorizedAccessException("Ordem de pagamento não encontrada.");

            if (order.IsPaid)
                throw new InvalidOperationException("Ordem de pagamento já está paga.");

            (bool paymentSucceeded, string reason) = await paymentGateway.SendPaymentRequest(user, orderId);

            if (!paymentSucceeded)
            {
                await eventStore.SaveAsync(new PaymentFailedEvent(order.Id)
                {
                    UserId = user.Id,
                    PaymentMethod = dto.Method.ToString(),
                    Reason = reason
                });

                return new PaymentResult()
                {
                    Success = false,
                    Message = "Pagamento falhou."
                };
            }

            order.MarkAsPaid();
            await orderRepository.UpdateAsync(order);

            var taskPopularity = gameService.IncreaseGamesPopularity(user, gameIds);
            var taskLibrary = userService.AddGamesInLibraryAsync(user, gameIds);
            var taskEvent = eventStore.SaveAsync(new OrderPaidEvent(order.Id)
            {
                UserId = user.Id,
                PaymentMethod = dto.Method.ToString(),
            });

            await Task.WhenAll(taskPopularity, taskLibrary, taskEvent);

            return new PaymentResult()
            {
                Success = true,
                Message = "Pagamento realizado com sucesso."
            };
        }
    }

}
