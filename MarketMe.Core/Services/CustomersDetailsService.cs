using MarketMe.Core.IServices;
using MarketMe.Core.MarketDbContexts;
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
        private MarketDbContext _marketDbContext;

        public CustomersDetailsService(IUnitOfWork uow, MarketDbContext marketDbContext) : base(uow)
        {
            _marketDbContext = marketDbContext;
        }

        public async Task<CustomersDetailsViewModel> AddCustomer_Entity(CustomersDetailsViewModel model)
        {

            if (model == null)
            {
                this.Results.Add(new ValidationResult($"Invalid model"));
                return null;
            }
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.PhoneNumber) || string.IsNullOrEmpty(model.FirstName))
            {
                this.Results.Add(new ValidationResult($"Kindly fill all the required field"));
                return null;
            }

            var customer = (CustomersDetails)model;
            _marketDbContext.CustomersDetails.Add(customer);
            _marketDbContext.SaveChanges();
            return model;

        }

        public async Task<CustomersDetailsViewModel> CustomerwithId_Entity(Guid id)
        {

            if (id == Guid.Empty)
            {
                this.Results.Add(new ValidationResult($"Invalid Id"));
                return null;
            }
            var cust = _marketDbContext.CustomersDetails.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();

            var customer = (CustomersDetailsViewModel)cust;
            return customer;
        }

        public async Task<CustomersDetailsViewModel> DeleteCustomer_Entity(Guid id)
        {
            if (id == Guid.Empty)
            {
                this.Results.Add(new ValidationResult($"Invalid Id"));
                return null;
            }
            var cust = _marketDbContext.CustomersDetails.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
            if (cust == null)
            {
                this.Results.Add(new ValidationResult($"Customer not found"));
                return null;
            }
            cust.IsDeleted = true;
            _marketDbContext.CustomersDetails.Update(cust);
            _marketDbContext.SaveChanges();
            var customer = (CustomersDetailsViewModel)cust;
            return customer;
        }

        public async Task<IEnumerable<CustomersDetails>> GetallCustomer_Entity()
        {
            var cust = _marketDbContext.CustomersDetails.Where(x => x.IsDeleted == false).ToList();

            return cust;
        }

        public Task<IEnumerable<CustomersDetailsViewModel>> GetCustomers()
        {
            throw new NotImplementedException();
        }

        public Task<CustomersDetailsViewModel> GetCustomerWithEmial(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomersDetailsViewModel> GetCustomerwithId(Guid id)
        {

            var user = this.SqlQuery<CustomersDetails>
          ("SELECT * FROM [CustomersDetails] i WHERE Id= @Id AND IsActive <>1 AND IsDeleted <>1",
           new
           {
               Id = id

           }).FirstOrDefault();

            if (user == null)
            {
                this.Results.Add(new ValidationResult($"Account not found"));
                return null;
            }

            var userDetail = (CustomersDetailsViewModel)user;
            return userDetail;

        }

        public async Task<CustomersDetailsViewModel> UpdateCustomer_Entity(CustomersDetailsViewModel model)
        {
            if (string.IsNullOrEmpty(model.Id) )
            {
                this.Results.Add(new ValidationResult($"Id  not found"));
                return null;
            }

            if(!Guid.TryParse(model.Id, out Guid id))
            {
                this.Results.Add(new ValidationResult($"Invalid Id"));
                return null;
            }
            var cust = _marketDbContext.CustomersDetails.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
            cust.FirstName = model.FirstName;
            cust.LastName = model.LastName;
            cust.Email = cust.ModifiedBy = model.Email;
            cust.ModifiedOn = DateTime.Now;
            _marketDbContext.CustomersDetails.Update(cust);
            _marketDbContext.SaveChanges();
            var customer = (CustomersDetailsViewModel)cust;
            return customer;
        }
    }
}
