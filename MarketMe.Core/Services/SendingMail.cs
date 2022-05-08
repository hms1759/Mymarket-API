//using MarketMe.Core.IServices;
//using Shared.Mail;
//using Shared.Notification.Email;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MarketMe.Core.Services
//{
//    public class SendingMail : ISendingMail
//    {
//        private readonly IEmailNotifier _emailService;

//        public SendingMail(IEmailNotifier emailService)
//        {
//            _emailService = emailService;
//        }

//        public Task<string> ReadTemplate(string template, string layoutTemplate)
//        {
//            //string filepath = $@"{ environment.WebRootPath}/App_Data/EmailNotifications";
//            string filepath = Path.Combine(Environment.CurrentDirectory, @"wwwroot\app_data\", "email_templates");
//            string htmlPath = Path.Combine(filepath, layoutTemplate);
//            string contentPath = Path.Combine(filepath, $"{template.ToLower()}.html");
//            string html = string.Empty;
//            string body = string.Empty;

//            // get global html template
//            if (File.Exists(htmlPath))
//                html = File.ReadAllText(htmlPath);
//            else
//                return null;

//            // get specific message content
//            if (File.Exists(contentPath))
//                body = File.ReadAllText(contentPath);
//            else
//                return null;

//            string msgBody = html.Replace("{Body}", body);
//            return Task.FromResult(msgBody);
//        }

//        public async Task SendEmailAsync(string to, string subject, Dictionary<string, string> messages, string template, string layoutTemplate = "_layout.html")
//        {
//            var msgBody = await ReadTemplate(template, layoutTemplate ?? "_layout.html");
//            var body = msgBody.ParseTemplate(messages);
//            //await _notificationService.SendEmailAsync(subject, body,
//            //   to);
//            ////"lexzy_dot_net@yahoo.co.uk");
//        }

//        public async Task SendManyEmailAsync(List<string> to, string subject, Dictionary<string, string> messages, string template, string layoutTemplate = "_layout.html")
//        {
//            var msgBody = await ReadTemplate(template, layoutTemplate ?? "_layout.html");
//            var body = msgBody.ParseTemplate(messages);
//            List<Task> job = new List<Task>();
//            //  to.ForEach(num => job.Add(Task.Run(() => _notificationService.SendEmailAsync(subject, body, num))));
//            await Task.WhenAll(job);
//        }

//    }
//}
