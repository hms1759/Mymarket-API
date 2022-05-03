using MarketMe.Share.Basic;
using MarketMe.Share.Extensions;
using MarketMe.Share.Utils.GUIDs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarketMe.Core.Models
{
    public class MailRecords : BaseEntity
    {
      
        public string Subject { get; set; }

        public string MessageType { get; set; }
        public string Receipient { get; set; }
        public bool IsRead { get; set; }
        [Required]
        public string Content { get; set; }
    }
}