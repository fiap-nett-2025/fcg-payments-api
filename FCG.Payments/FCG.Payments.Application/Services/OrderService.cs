using FCG.Payments.Application.DTO.Order;
using FCG.Payments.Application.Services.Interfaces;
using FCG.Payments.Domain.Events.Order;
using FCG.Payments.Infra.Data.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace FCG.Payments.Application.Services
{
    public class OrderService(
        ILogger<OrderService> logger,
        IOrderRepository orderRepository,
        IEventStore eventStore,
        IUserService userService,
        IPaymentGatewayService paymentGateway
    ) : IOrderService
    {
        public async Task<OrderDto?> GetOrderAsync(Guid orderId)
        {
            var order = await orderRepository.GetByIdAsync(orderId);
            return order == null ? null : OrderDto.ToDto(order);
        }

        public async Task<PaymentResult> PayOrderAsync(Guid userId, Guid orderId, PayOrderDto dto)
        {
            logger.LogDebug("Processing payment for order {OrderId} by user {UserId}", orderId, userId);
            var order = await orderRepository.GetByIdAsync(orderId)
                       ?? throw new InvalidOperationException("Order not found");

            var gameIds = order.Items.Select(i => i.GameId).ToArray();
            logger.LogDebug("Order {OrderId} has {ItemCount} games: {GamesIds}", orderId, order.Items.Count, string.Join(", ", gameIds));

            if (order.UserId != userId)
                throw new UnauthorizedAccessException("Not your order");

            if (order.IsPaid)
                throw new InvalidOperationException("Order already paid");

            var paymentSucceeded = await paymentGateway.SendPaymentRequest(); 

            if (!paymentSucceeded)
            {
                await eventStore.SaveAsync(new PaymentFailedEvent
                {
                    UserId = userId,
                    OrderId = order.Id,
                    PaymentMethod = dto.Method,
                    Reason = "Payment gateway declined"
                });

                return new PaymentResult()
                {
                    Success = false,
                    Message = "Payment failed"
                };
            }

            order.MarkAsPaid();
            await orderRepository.UpdateAsync(order);

            var taskLibrary = userService.AddGamesInLibraryAsync(userId, gameIds);
            var taskEvent = eventStore.SaveAsync(new OrderPaidEvent
            {
                OrderId = order.Id,
                PaymentMethod = dto.Method
            });

            await Task.WhenAll(taskLibrary, taskEvent);

            return new PaymentResult()
            {
                Success = true,
                Message = "Payment successful"
            };
        }
    }

}
