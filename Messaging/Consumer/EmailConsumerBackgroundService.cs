
namespace SimpleMinimalAPI.Messaging.Consumer
{
    public class EmailConsumerBackgroundService : BackgroundService
    {
        private readonly EmailConsumer _emailConsumer;

        public EmailConsumerBackgroundService(EmailConsumer emailConsumer)
        {
            _emailConsumer = emailConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _emailConsumer.Consume(stoppingToken);
        }
    }
}
