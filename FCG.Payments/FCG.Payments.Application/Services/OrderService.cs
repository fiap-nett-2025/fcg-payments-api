using FCG.Payments.Application.DTO.Order;
using FCG.Payments.Application.Services.Interfaces;
using FCG.Payments.Data.Repository;
using FCG.Payments.Data.Repository.Interfaces;
using FCG.Payments.Domain.Entities;
using FCG.Payments.Domain.Enums;
using FCG.Payments.Domain.Events.Order;

namespace FCG.Payments.Application.Services
{
    public class OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IEventStore eventStore) : IOrderService
    {
        public async Task<OrderDto> CreateOrderFromCartAsync(Guid userId)
        {
            var cart = await cartRepository.GetByUserIdAsync(userId)
                       ?? throw new InvalidOperationException("Cart not found");

            if (cart.Items.Count == 0)
                throw new InvalidOperationException("Cart is empty");

            var order = new Order(userId, cart.Items);

            //if (cart.DiscountValue > 0)
            //{
            //    order.ApplyDiscount(cart.DiscountValue);
            //}

            await orderRepository.AddAsync(order);

            await eventStore.SaveAsync(new OrderCreatedEvent
            {
                OrderId = order.Id,
                UserId = userId,
                TotalPrice = order.Total
            });

            // depois de criar o pedido, limpa o carrinho
            cart.Clear();
            await cartRepository.UpdateAsync(cart);

            return ToDto(order);
        }

        public async Task<OrderDto?> GetOrderAsync(Guid orderId)
        {
            var order = await orderRepository.GetByIdAsync(orderId);
            return order == null ? null : ToDto(order);
        }

        public async Task<PaymentResult> PayOrderAsync(Guid userId, Guid orderId, PayOrderDto dto)
        {
            var order = await orderRepository.GetByIdAsync(orderId)
                       ?? throw new InvalidOperationException("Order not found");

            if (order.UserId != userId)
                throw new UnauthorizedAccessException("Not your order");

            if (order.IsPaid)
                throw new InvalidOperationException("Order already paid");

            if(dto.Discount != null)
            {
                order.ApplyDiscount(dto.Discount.Value);
                await eventStore.SaveAsync(new OrderDiscountAppliedEvent
                {
                    OrderId = order.Id,
                    Discount = dto.Discount,
                });
            }

            // simulação do pagamento
            var paymentSucceeded = true; // aqui você chamaria o gateway de pagamento

            if (paymentSucceeded)
            {
                order.MarkAsPaid();
                await orderRepository.UpdateAsync(order);

                await eventStore.SaveAsync(new OrderPaidEvent
                {
                    OrderId = order.Id,
                    PaymentMethod = dto.Method
                });

                return new PaymentResult()
                {
                    Success = true,
                    Message = "Payment successful"
                };
            }

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

        private static OrderDto ToDto(Order order)
        {
            var dto = new OrderDto()
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(i => new OrderItemDto()
                {
                    GameId = i.GameId,
                    Price = i.UnitPrice

                }).ToList(),
                Status = order.Status,
                Total = order.Total,
            };
            return dto;
        }
    }

}
