using RabbitMQ.Client;
using SimpleMinimalAPI.Contracts.Messaging;
using System.Text;
using System.Text.Json;

namespace SimpleMinimalAPI.Messaging.Producer
{
    public class EmailProducer : IProducer
    {
        private readonly IChannel _channel;

        public EmailProducer(IConnectionFactory connectionFactory)
        {
            var connection = connectionFactory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = connection.CreateChannelAsync().GetAwaiter().GetResult();
        }


        public async Task Publish<T>(T message)
        {
            await _channel.ExchangeDeclareAsync(exchange: "send-email", type: "direct");
            await _channel.QueueDeclareAsync("email-queue", durable: true, exclusive: true);
            await _channel.QueueBindAsync(queue: "email-queue", exchange: "send-email", routingKey: "");

            var jsonString = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonString);

            await _channel.BasicPublishAsync(exchange: "send-email", routingKey: "", body: body);
        }
    }
}
