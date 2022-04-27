using System;
using System.Collections.Generic;
using System.Text;

namespace MarketMe.Share.Notification
{
    public class AppConfiguration
    {
        public string GeneralContact { get; set; }
        public MailSetting EmailSettings { get; set; }
    }

    public class MailSetting
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string TestSetting { get; set; }
        public string ApiKey { get; set; }
        public string Email { get; set; }
        public string FeedBackEmail { get; set; }
    }
}