using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Core.Notification
{
    public interface IMessagingService
    {
        
        Task<dynamic> AddEmailService(string recipeintEmail, string subject, Dictionary<string, string> messagePlaceHolders, string messageType, string layout);
        void Send(dynamic data);
    }
}
