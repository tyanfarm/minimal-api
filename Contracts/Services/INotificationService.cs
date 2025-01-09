using SimpleMinimalAPI.Models;

namespace SimpleMinimalAPI.Contracts.Services
{
    public interface INotificationService
    {
        Task<bool> SendNotification(MessageRequest message, string additionalParam);
    }
}
