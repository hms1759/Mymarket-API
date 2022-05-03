using MarketMe.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Core.ViewModels
{
    public class CustomersDetailsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string BusinessName { get; set; }
        public string BusinessEmail { get; set; }
        public bool  IsActive { get; set; }

        [Range(100, 500, ErrorMessage = "Please enter correct value")]
        public string BusinessAddress { get; set; }


        public static explicit operator CustomersDetailsViewModel(CustomersDetails source)
        {
            var destination = new CustomersDetailsViewModel();
            destination.Address = source.Address;
            destination.BusinessAddress = source.BusinessAddress;
            destination.BusinessEmail = source.BusinessEmail;
            destination.BusinessName = source.BusinessName;
            destination.Email = source.Email;
            destination.FirstName = source.FirstName;
            destination.LastName = source.LastName;
            destination.PhoneNumber = source.PhoneNumber;
            destination.IsActive = source.IsActive;
            return destination;
        }

        public static explicit operator CustomersDetails(CustomersDetailsViewModel source)
        {
            var destination = new CustomersDetails();
            destination.Address = source.Address;
            destination.BusinessAddress = source.BusinessAddress;
            destination.BusinessEmail = source.BusinessEmail;
            destination.BusinessName = source.BusinessName;
            destination.Email = source.Email;
            destination.FirstName = source.FirstName;
            destination.LastName = source.LastName;
            destination.PhoneNumber = source.PhoneNumber;
            destination.IsActive = source.IsActive;
            return destination;
        }
    }
}