using MarketMe.Share.Basic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Core.Models
{
  public  class CustomersDetails : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string  Email { get; set; }
        [StringLength( 500)]
        public string Address { get; set; }
        public string BusinessName { get; set; }
        public string BusinessEmail { get; set; }
        [StringLength(500)]
        public string BusinessAddress { get; set; }
        public bool IsActive { get; set; }

    }
}
