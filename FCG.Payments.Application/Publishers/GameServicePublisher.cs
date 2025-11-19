using FCG.Payments.Application.DTO.Game;
using FCG.Payments.Application.Publishers.Interfaces;
using FCG.Payments.Domain.Messaging.Interfaces;
using FCG.Payments.Infra.Messaging.Config;
using Microsoft.Extensions.Options;

namespace FCG.Payments.Application.Publishers
{
    public class GameServicePublisher(
        IQueuePublisher publisher, 
        IOptions<ExchangesOptions> exchanges,
        IOptions<QueuesOptions> queues
    ) : IGameServicePublisher
    {
        public Task IncreasePopularityAsync(IncreaseGamePopularityDTO dto)
        {
            return publisher.PublishAsync(
                dto,
                exchanges.Value.GameExchange,
                queues.Value.GamePopularityIncreasedQueue
            );
        }
    }
}
