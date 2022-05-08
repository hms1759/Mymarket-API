using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Share.Basic
{
   public class Basics
    {
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
       public string DeletedBy { get; set; }
        public DateTime DeletedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get;  set; }
    }
}
