using MarketMe.Core.Models;
using Shared.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Core.IServices
{
   public interface IUserService :IService<CustomersDetails>
    {
        Task<RegistrationViewModel> UserRegistration(RegistrationViewModel model);
        Task UserRegistration(string id);
    }
}
