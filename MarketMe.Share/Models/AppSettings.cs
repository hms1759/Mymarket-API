using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Share.Models
{
    public class AppSettings
    {
        public string AdminEmail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string ApiKey { get; set; }
        public string Email { get; set; }
        public string FeedbackEmail { get; set; }
    }

}
