using MarketMe.Core.Models;
using MarketMe.Core.ViewModels;
using Shared.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Core.IServices
{
    public interface IUserService : IService<CustomersDetails>
    {
        Task<tokenViewModel> AccountLogin(LoginViewModel model);
        Task<RegistrationViewModel> UserRegistration(RegistrationViewModel model);
        Task<CustomersDetailsViewModel> accountVerificationAsync(AccountVerificationViewModel model);
    }
}
