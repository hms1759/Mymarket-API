using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Core.ViewModels
{
    public class MailRecordViewModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Subject { get; set; }
        public string Receipient { get; set; }
        public string MessageType { get; set; }
        public bool IsRead { get; set; }
        public string Content { get; set; }
    }
}
