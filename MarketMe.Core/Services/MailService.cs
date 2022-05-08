using System;
using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MarketMe.Core;
using MarketMe.Core.IServices;
using MarketMe.Core.Models;
using MarketMe.Share.Models;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MarketMe.Core.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        private readonly AppSettings _appSettings;
        public MailService(IOptions<MailSettings> mailSettings, AppSettings appSettings)
        {
            _mailSettings = mailSettings.Value;
            _appSettings = appSettings;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Sender = MailboxAddress.Parse(_appSettings.AdminEmail);
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }


            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_appSettings.Host, _appSettings.Port, SecureSocketOptions.SslOnConnect);
            smtp.Authenticate(_appSettings.AdminEmail, _appSettings.Password);
            try
            {
                await smtp.SendAsync(email);

            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return;
            }
            smtp.Disconnect(true);
        }


    }
}