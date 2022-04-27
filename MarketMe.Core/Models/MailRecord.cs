using MarketMe.Share.Basic;
using MarketMe.Share.Extensions;
using MarketMe.Share.Utils.GUIDs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketMe.Core.Models
{
    public class MailRecord : Entity, IDateAudit
    {
        public MailRecord()
        {
            this.Id = SequentialGuidGenerator.Instance.Create();
            CreatedOn = DateTime.Now.GetDateUtcNow().ToLocalTime();
        }

        // public Guid Id { get; set; }
        public string Subject { get; set; }

        public string MessageType { get; set; }
        public string Receipient { get; set; }
        public bool IsRead { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}