using FCG.Payments.Application.DTO.Game;
using FCG.Payments.Domain.Messaging.Interfaces;
using Microsoft.Extensions.Logging;

namespace FCG.Payments.Application.Handlers
{
    public class GameTestMessageHandler(ILogger<GameTestMessageHandler> logger) : IMessageHandler<GameDto>
    {
        public Task HandleAsync(GameDto message, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Received game message: {Game}", message);
            return Task.CompletedTask;
        }
    }
}
