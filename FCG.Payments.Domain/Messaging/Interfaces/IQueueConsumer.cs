namespace FCG.Payments.Domain.Messaging.Interfaces
{
    public interface IQueueConsumer
    {
        Task StartAsync<T>(string queueName, IMessageHandler<T> handler, 
            CancellationToken cancellationToken = default);
    }
}
