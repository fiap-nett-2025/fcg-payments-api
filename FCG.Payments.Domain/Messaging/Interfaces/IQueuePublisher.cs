namespace FCG.Payments.Domain.Messaging.Interfaces
{
    public interface IQueuePublisher
    {
        Task PublishAsync<T>(T message, string queueName, string? exchange = null, CancellationToken cancellationToken = default);
    }
}
