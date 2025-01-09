using SimpleMinimalAPI.Contracts.Services;
using SimpleMinimalAPI.Helper;
using SimpleMinimalAPI.Models;
using System.Net;
using System.Net.Mail;

namespace SimpleMinimalAPI.Services
{
    public class EmailService : INotificationService
    {
        private static string? EMAIL = AppSettings.Instance?.EmailInfo.Email;
        private static string? PASSWORD = AppSettings.Instance?.EmailInfo.Password;

        public async Task<bool> SendNotification(MessageRequest message, string emailDestination)
        {
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(EMAIL, PASSWORD),
                    EnableSsl = true,
                };

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(EMAIL),
                    To = { emailDestination },
                    Subject = message.Title,
                    Body = message.Message,
                    IsBodyHtml = true,
                };

                await client.SendMailAsync(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");

                return false;
            }
        }
    }
}
