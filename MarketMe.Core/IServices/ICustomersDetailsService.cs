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
    public interface ICustomersDetailsService : IService<CustomersDetails>
    {
        Task<IEnumerable<CustomersDetailsViewModel>> GetCustomers();
        Task<CustomersDetailsViewModel> GetCustomerwithId(Guid id);
        Task<CustomersDetailsViewModel> GetCustomerWithEmial(string email);
        #region Entity Framework
        Task<CustomersDetailsViewModel> AddCustomer_Entity(CustomersDetailsViewModel model);
        Task<IEnumerable<CustomersDetails>> GetallCustomer_Entity();
        Task<CustomersDetailsViewModel> CustomerwithId_Entity(Guid id);
        Task<CustomersDetailsViewModel> DeleteCustomer_Entity(Guid id);
        Task<CustomersDetailsViewModel> UpdateCustomer_Entity(CustomersDetailsViewModel model);

        #endregion

    }
}
