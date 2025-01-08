using SimpleMinimalAPI.Models;

namespace SimpleMinimalAPI.Contracts
{
    public interface INotificationService
    {
        Task<bool> SendNotification(MessageRequest message, string additionalParam);
    }
}
