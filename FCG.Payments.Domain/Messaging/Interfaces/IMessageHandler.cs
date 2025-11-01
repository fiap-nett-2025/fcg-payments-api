namespace FCG.Payments.Domain.Messaging.Interfaces
{
    public interface IMessageHandler<T>
    {
        Task HandleAsync(T message, CancellationToken cancellationToken = default);
    }
}
