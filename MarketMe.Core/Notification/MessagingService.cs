using Microsoft.Extensions.Logging;
using Shared.DI;
using Shared.Helpers;
using Shared.Notification.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Notification
{
    public class MessagingService
    {
        private readonly IEmailNotifier _emailNotifier;

        public MessagingService(IEmailNotifier emailNotifier)
        {
            _emailNotifier = emailNotifier;
        }

       public List<Action<IEmailNotifier>> NotificationServices { get; } = new List<Action<IEmailNotifier>>();

        private MessagingService()
        {
        }

        public static MessagingService Initialize()
        {
            return new MessagingService();
        }

        public MessagingService AddEmailService(string recipeintEmail, string subject, Dictionary<string, string> messagePlaceHolders, string messageType, string layout)
        {
          dynamic operation =  _emailNotifier.SendEmailAsync(recipeintEmail, subject, messagePlaceHolders, messageType, layout);

            //Action<IEmailNotifier> operation = async (_emailNotifier) =>
            //{
            //    try
            //    {
            //        await _emailNotifier.SendEmailAsync(recipeintEmail, subject, messagePlaceHolders, messageType, layout);
            //    }
            //    catch (Exception ex)
            //    {
            //        var _logger = ServiceLocator.Current.GetInstance<ILogger<MessagingService>>();
            //        _logger.LogError(CustomEventIds.SendItem, ex, null);
            //    }
            //};

          NotificationServices.Add(operation);

            return this;
        }

        public void Send()
        {
            var tasks = new List<Task>();

            foreach (var service in NotificationServices)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    var _emailNotifier = ServiceLocator.Current.GetInstance<IEmailNotifier>();
                    service(_emailNotifier);
                }));
            }

            Task.WaitAll(tasks.ToArray());
        }
      

    }
}
