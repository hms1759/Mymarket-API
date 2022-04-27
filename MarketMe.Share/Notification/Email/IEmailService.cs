using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Share.Notification.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string subject, string message, string toEmail, string name);

        Task SendEmailAsync(string subject, string message, string toEmail);

        Task SendManyEmailAsync(string subject, string message, List<string> toEmails);
    }
}