namespace FCG.Payments.Domain.Messaging.Interfaces
{
    public interface IQueuePublisher
    {
        Task PublishAsync<T>(T message, string exchange, string queueName, CancellationToken cancellationToken = default);
    }
}
