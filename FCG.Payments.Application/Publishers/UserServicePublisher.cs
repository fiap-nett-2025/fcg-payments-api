using FCG.Payments.Application.DTO.Game;
using FCG.Payments.Application.Publishers.Interfaces;
using FCG.Payments.Domain.Messaging.Interfaces;
using FCG.Payments.Infra.Messaging.Config;
using Microsoft.Extensions.Options;

namespace FCG.Payments.Application.Publishers
{
    public class UserServicePublisher(
        IQueuePublisher publisher,
        IOptions<ExchangesOptions> exchanges,
        IOptions<QueuesOptions> queues
    ) : IUserServicePublisher
    {
        public Task AddGamesInLibraryAsync(AddGameInLibraryDTO dto)
        {
            return publisher.PublishAsync(
                dto,
                exchanges.Value.UserExchange,
                queues.Value.UserGameLibraryAddedQueue
            );
        }
    }
}
