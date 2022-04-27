using MarketMe.Share.Notification.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Share.Notification

{
    public class EmailNotification : INotifier
    {
        private readonly ILogger<EmailNotification> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailSvc;
        private readonly IWebHostEnvironment _env;

        public EmailNotification(IEmailService emailSvc, 
            ILogger<EmailNotification> logger, IHttpContextAccessor httpContextAccessor)
        {
            _emailSvc = emailSvc;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<string> ReadTemplate(string messageType)
        {
            string htmlPath = Path.Combine(ApplicationEnvironment.ApplicationBasePath, @"wwwroot\html", "_template.html");
            string contentPath = Path.Combine(ApplicationEnvironment.ApplicationBasePath, @"wwwroot\html", $"{messageType}.txt");
            string html = string.Empty;
            string body = string.Empty;

            // get global html template
            if (File.Exists(htmlPath))
                html = File.ReadAllText(htmlPath);
            else
                return null;

            // get specific message content
            if (File.Exists(contentPath))
                body = File.ReadAllText(contentPath);
            else return null;

            string msgBody = html.Replace("{body}", body);
            return Task.FromResult(msgBody);
        }

        public Task ReadTemplate(object trainerApplicant)
        {
            throw new NotImplementedException();
        }

        public async Task SendAsync(string to, string subject, string body)
        {

            await _emailSvc.SendEmailAsync(subject, body, to);
        }

        public async Task SendManyAsync(List<string> to, string subject, string body)
        {
            await _emailSvc.SendManyEmailAsync(subject, body, to);
        }
    }
}