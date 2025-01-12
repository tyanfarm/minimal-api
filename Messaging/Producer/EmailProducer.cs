using RabbitMQ.Client;
using SimpleMinimalAPI.Contracts.Messaging;
using System.Text;
using System.Text.Json;

namespace SimpleMinimalAPI.Messaging.Producer
{
    public class EmailProducer : IProducer
    {
        private readonly IConnection _connection;

        public EmailProducer(IConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CreateConnectionAsync().GetAwaiter().GetResult();
        }


        public async Task Publish<T>(T message)
        {
            using (var channel = await _connection.CreateChannelAsync())
            {
                await channel.ExchangeDeclareAsync(exchange: "send-email", type: "direct");
                await channel.QueueDeclareAsync("email-queue", durable: true, exclusive: false);
                await channel.QueueBindAsync(queue: "email-queue", exchange: "send-email", routingKey: "");

                var jsonString = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(jsonString);

                await channel.BasicPublishAsync(exchange: "send-email", routingKey: "", body: body);
            }
        }
    }
}
