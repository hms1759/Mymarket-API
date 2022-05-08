
using MarketMe.Share.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Shared.Notification.Email
{
    public class EmailService : IEmailService
    {
        //private  EmailSetting _settings;
        //public EmailService(EmailSetting settings)
        //{
        //    _settings = settings;
        //}

        private readonly AppSettings _settings;
        public EmailService(AppSettings settings)
        {
            _settings = settings;
        }

        public Task SendEmailAsync(string subject, string message, string toEmail, string name)
        {
            return Execute(_settings.ApiKey, subject, message, _settings.Email, new List<string> { toEmail }, name);
        }

        public Task SendEmailAsync(string subject, string message, string toEmail)
        {
            return Execute(_settings.ApiKey, subject, message, _settings.Email, new List<string> { toEmail });
        }

        public Task SendManyEmailAsync(string subject, string message, List<string> toEmails)
        {
            return Execute(_settings.ApiKey, subject, message, _settings.Email, toEmails);
        }

        public Task Execute(string apiKey, string subject, string message, string fromEmail, List<string> toEmails, string name = "")
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
            toEmails.ForEach(email =>
            {
                msg.AddTo(new EmailAddress(email));
            });
            return client.SendEmailAsync(msg);
        }
   
    }
}
