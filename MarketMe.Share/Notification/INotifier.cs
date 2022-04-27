using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Share.Notification
{
    public interface INotifier
    {
        Task<string> ReadTemplate(string messageType);

        Task SendAsync(string to, string subject, string body);

        Task SendManyAsync(List<string> to, string subject, string body);
        /// Task ReadTemplate(object trainerApplicant);
    }
}