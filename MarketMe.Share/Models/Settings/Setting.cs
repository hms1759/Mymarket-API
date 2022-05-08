using MarketMe.Share.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Share.Models.Settings
{
    public class Setting : BaseEntity
    {
        public bool IsDefault { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        //public int DataType { get; set; }
        public Guid? Business_Id { get; set; }
        public Guid? Store_Id { get; set; }
    }
}
