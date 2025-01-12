namespace SimpleMinimalAPI.Contracts.Messaging
{
    public interface IConsumer
    {
        Task Consume(CancellationToken cancellationToken);
    }
}
