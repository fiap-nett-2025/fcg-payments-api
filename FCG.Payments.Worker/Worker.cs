using FCG.Payments.Application.DTO.Game;
using FCG.Payments.Domain.Messaging.Interfaces;
using FCG.Payments.Infra.Messaging.Config;
using Microsoft.Extensions.Options;

namespace FCG.Payments.Worker
{
    public class Worker(ILogger<Worker> logger, IOptions<QueuesOptions> queuesOptions, IQueueConsumer consumer, IMessageHandler<GameDto> messageHandler) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await consumer.StartAsync(queuesOptions.Value.GameTestQueue, messageHandler, stoppingToken);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error on Worker: {ErrorMessage}", ex.Message);
            }
            finally
            {
                logger.LogInformation("Worker stopped at: {time}", DateTimeOffset.Now);
            }
        }
    }
}
