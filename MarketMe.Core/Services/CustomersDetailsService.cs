using MarketMe.Core.IServices;
using MarketMe.Core.Models;
using MarketMe.Core.ViewModels;
using Shared.Dapper;
using Shared.Dapper.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Core.Services
{
    public class CustomersDetailsService : Service<CustomersDetails>, ICustomersDetailsService
    {
        protected List<ValidationResult> results = new List<ValidationResult>();

        public CustomersDetailsService(IUnitOfWork uow) : base(uow)
        {

        }
        public Task<IEnumerable<CustomersDetailsViewModel>> GetCustomers()
        {
            throw new NotImplementedException();
        }

        public async Task<CustomersDetailsViewModel> GetCustomerwithId(Guid id)
        {

            var user = this.SqlQuery<CustomersDetailsViewModel>
          ("SELECT * FROM [CustomersDetails] i WHERE Id= @Id AND IsActivate <>1 AND IsDeleted <>1",
       new
       {
           Id = id

       }).FirstOrDefault();

            if (user == null)
            {
                this.Results.Add(new ValidationResult($"Account not found"));
                return null;
            }
            return user;

        }
    }
}
