namespace SimpleMinimalAPI.Contracts.Messaging
{
    public interface IProducer
    {
        Task Publish<T> (T message);
    }
}
