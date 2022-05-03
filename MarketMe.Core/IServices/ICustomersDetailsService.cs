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

    }
}
