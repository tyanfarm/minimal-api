using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SimpleMinimalAPI.Contracts.Messaging;
using SimpleMinimalAPI.Models;
using SimpleMinimalAPI.Services;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace SimpleMinimalAPI.Messaging.Consumer
{
    public class EmailConsumer : IConsumer
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly EmailService _emailService;

        public EmailConsumer(IConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
            _emailService = new EmailService();
        }

        public async Task Consume(CancellationToken cancellationToken)
        {
            await _channel.ExchangeDeclareAsync(exchange: "send-email", type: "direct");
            await _channel.QueueDeclareAsync("email-queue", durable: true, exclusive: false);
            await _channel.QueueBindAsync(queue: "email-queue", exchange: "send-email", routingKey: "");

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var jsonString = Encoding.UTF8.GetString(body);

                try
                {
                    // Deserialize message & emailDestination
                    var data = JsonSerializer.Deserialize<dynamic>(jsonString);

                    // Extract "EmailMessage" and "EmailDestination" fields
                    var emailMessageElement = data?.GetProperty("EmailMessage");
                    var emailDestination = data?.GetProperty("EmailDestination").GetString();

                    // Deserialize "EmailMessage" to MessageRequest
                    var message = JsonSerializer.Deserialize<MessageRequest>(emailMessageElement?.GetRawText());


                    if (message == null || string.IsNullOrEmpty(emailDestination))
                    {
                        throw new Exception("Invalid message or email destination");
                    }

                    var emailSent = await _emailService.SendNotification(message, emailDestination);

                    await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");

                    await _channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: true);
                }
            };

            await _channel.BasicConsumeAsync(queue: "email-queue", autoAck: false, consumer: consumer);

            // Keep listening until the application is stopped
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
