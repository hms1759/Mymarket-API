using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Notification.Email
{
    public interface IEmailNotifier 
    {
        Task<string> ReadTemplate(string template, string layoutTemplate = "_template.html");//MessageTypes messageTypes);
        Task SendEmailAsync(string to, string subject, Dictionary<string, string> message, string template, string layoutTemplate = "_layout.html");
        Task SendManyEmailAsync(List<string> to, string subject, Dictionary<string, string> messages, string template, string layoutTemplate = "_layout.html");

    }
}
