using MarketMe.Core.IServices;
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

        public UserService(UserManager<IdentityUser> userManager, IRegexValidation regexValidation,IUnitOfWork uow) : base(uow)
        {
            _userManager = userManager; 
            _regexValidation = regexValidation;
        }
        public async Task UserRegistration(RegistrationViewModel model)
        {
            if (model == null)
            {
                this.Results.Add(new ValidationResult($"Invalid model"));
                return;
            }

            if (!_regexValidation.PasswordValidation(model.Password))
            {
                this.Results.Add(new ValidationResult($"Invalid password: password should like Ade@1234"));
                return;

            };

        
            var identityUser = new IdentityUser
            {
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                UserName = model.Email,

            };

            var result = await _userManager.CreateAsync(identityUser, model.Password);


            if (!result.Succeeded)
            {
                var error = _userManager.ErrorDescriber;
                this.Results.Add(new ValidationResult(error.ToString()));
                return;
            }

            return;
        }

        public Task UserRegistration(string id)
        {
            throw new NotImplementedException();
        }
    }
}
