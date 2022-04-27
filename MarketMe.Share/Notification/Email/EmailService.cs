

using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMe.Share.Notification.Email
{
    public class EmailService : IEmailService
    {
        public EmailService(AppConfiguration settings)
        {
            _settings = settings;
        }

        private AppConfiguration _settings;

        public Task SendEmailAsync(string subject, string message, string toEmail, string name)
        {
            return Execute(_settings.EmailSettings.ApiKey, subject, message, _settings.EmailSettings.Email, new List<string> { toEmail }, name);
        }

        public Task SendEmailAsync(string subject, string message, string toEmail)
        {
            return Execute(_settings.EmailSettings.ApiKey, subject, message, _settings.EmailSettings.Email, new List<string> { toEmail });
        }

        public Task SendManyEmailAsync(string subject, string message, List<string> toEmails)
        {
            return Execute(_settings.EmailSettings.ApiKey, subject, message, _settings.EmailSettings.Email, toEmails);
        }

        public async Task<SendGrid.Response> Execute(string apiKey, string subject, string message, string fromEmail, List<string> toEmails, string name = "")
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = string.IsNullOrEmpty(name) ? new EmailAddress(fromEmail) : new EmailAddress(fromEmail, name),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            msg.SetClickTracking(false, false);
            var mails = toEmails.Select(email => new EmailAddress(email)).ToList();
            msg.AddTos(mails);
            try
            {
                return await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

      
    }
}