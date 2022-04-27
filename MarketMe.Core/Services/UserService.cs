using MarketMe.Core.IServices;
using MarketMe.Core.MarketDbContexts;
using MarketMe.Core.Models;
using MarketMe.Share.Validation;
using Microsoft.AspNetCore.Identity;
using Shared.Dapper;
using Shared.Dapper.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarketMe.Core.Services
{
    public class UserService : Service<CustomersDetails>, IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRegexValidation _regexValidation;
        private readonly ICustomersDetailsService _customersService;
        private MarketDbContext _dbContext;

        public UserService(UserManager<IdentityUser> userManager, MarketDbContext dbContext, IRegexValidation regexValidation,
                                ICustomersDetailsService customersService, IUnitOfWork uow) : base(uow)
        {
            _userManager = userManager;
            _regexValidation = regexValidation;
            _customersService = customersService;
            _dbContext = dbContext;
        }
        public async Task<RegistrationViewModel> UserRegistration(RegistrationViewModel model)
        {
            if (model == null)
            {
                this.Results.Add(new ValidationResult($"Invalid model"));
                return null;
            }

            var user = this.SqlQuery<IdentityUser>
                ("SELECT * FROM [AspNetUsers] i WHERE Email= @Email AND PhoneNumber = @Phone",
             new
             {
                 Email = model.Email,
                 Phone = model.PhoneNumber

             }).FirstOrDefault();

            if(user !=null)
            {
                this.Results.Add(new ValidationResult($"Email  {model.Email} or PhoneNumber {model.PhoneNumber} Existed already"));
                return null;
            }

            var Validpassword = _regexValidation.PasswordValidation(model.Password);
            if (!Validpassword)
            {
                this.Results.Add(new ValidationResult(_regexValidation.Errors[0]));
                return null;
            }

            var ValidEmail = _regexValidation.EmailValidation(model.Email);
            if (!ValidEmail)
            {
                this.Results.Add(new ValidationResult($"Invalid Email"));
                return null;
            };


            var identityUser = new IdentityUser
            {
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                UserName = model.Email,

            };
            var customerDetail = new CustomersDetails();
            customerDetail.Email = model.Email;
            customerDetail.PhoneNumber = model.PhoneNumber;
            await _customersService.AddAsync(customerDetail);

            var result = await _userManager.CreateAsync(identityUser, model.Password);


            if (!result.Succeeded)
            {
                var error = _userManager.ErrorDescriber;
                this.Results.Add(new ValidationResult(error.ToString()));
                return null;
            }


            return model;
        }

        public Task UserRegistration(string id)
        {
            throw new NotImplementedException();
        }
    }
}
