using FCG.Payments.Domain.Messaging.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace FCG.Payments.Infra.Messaging.Rabbit
{
    public class RabbitMqPublisher(ConnectionFactory factory) : IQueuePublisher
    {
        public async Task PublishAsync<T>(T message, string queueName, string? exchange = null, CancellationToken cancellationToken = default)
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            using IConnection connection = await factory.CreateConnectionAsync(cancellationToken);
            using IChannel channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);

            await channel.BasicPublishAsync(exchange ?? "", routingKey: queueName, body, cancellationToken: cancellationToken);
        }
    }
}
