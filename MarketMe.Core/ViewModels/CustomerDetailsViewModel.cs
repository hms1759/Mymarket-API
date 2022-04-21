using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Core.ViewModels
{
    class CustomerDetailsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string BusinessName { get; set; }
        public string BusinessEmail { get; set; }

        [Range(100, 500, ErrorMessage = "Please enter correct value")]
        public string BusinessAddress { get; set; }

    }
}