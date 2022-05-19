using MarketMe.Core.Constant;
using MarketMe.Core.IServices;
using MarketMe.Core.MarketDbContexts;
using MarketMe.Core.Models;
using MarketMe.Core.Notification;
using MarketMe.Core.ViewModels;
using MarketMe.Share.Constants;
using MarketMe.Share.Extensions;
using MarketMe.Share.Models;
using MarketMe.Share.Utils;
using MarketMe.Share.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Dapper;
using Shared.Dapper.Interfaces;
using Shared.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace MarketMe.Core.Services
{
    public class UserService : Service<CustomersDetails>, IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRegexValidation _regexValidation;
        private MarketDbContext _dbContext;
        private readonly AppSettings _appSettings;
        private readonly IMailRecordService _mailRecordService;
        private readonly IMailService _mailService;
        //private readonly IMessagingService _messagingService;
        private readonly ICacheService _cacheService;
        private IConfiguration _configuration;
        public UserService(UserManager<IdentityUser> userManager, MarketDbContext dbContext, IRegexValidation regexValidation,
                                ICustomersDetailsService customersService, IUnitOfWork uow, IMailRecordService mailRecordService,
                                IConfiguration configuration, AppSettings appSettings, IMailService mailService, ICacheService cacheService) : base(uow)
        {
            _userManager = userManager;
            _regexValidation = regexValidation;
            _dbContext = dbContext;
            _appSettings = appSettings;
            _mailRecordService = mailRecordService;
            _mailService = mailService;
            _cacheService = cacheService;
            _configuration = configuration;
        }
        public async Task<RegistrationViewModel> UserRegistration(RegistrationViewModel model)
        {
            if (model == null)
            {
                this.Results.Add(new ValidationResult($"Invalid model"));
                return null;
            }

            var user = this.SqlQuery<IdentityUser>
                ("SELECT * FROM [AspNetUsers] i WHERE Email= @Email OR UserName= @Email OR  PhoneNumber = @Phone",
             new
             {
                 Email = model.Email,
                 Phone = model.PhoneNumber

             }).FirstOrDefault();

            if (user != null)
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


            var User = new IdentityUser
            {
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                UserName = model.Email,

            };

            var customerDetail = new CustomersDetails();
            customerDetail.Email = model.Email;
            customerDetail.PhoneNumber = model.PhoneNumber;
          //  customerDetail.IsActive = true;

            var result = await _userManager.CreateAsync(User, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.ToArray()[0];
                var error = errors.Description;
                this.Results.Add(new ValidationResult(error));
                return null;
            }

            var token = RandomGenerator.GenerateAPPKeys(model.Email);

            var tokenList = new tokenViewModel();
            tokenList.Email = model.Email;
            tokenList.Token = token;
            tokenList.ExpireOn = DateTime.Now.AddMinutes(15);

            _cacheService.SetCacheData(tokenList);

            var request = new MailRequest();
            request.ToEmail = model.Email;
            request.Body = token;
            request.Subject = Constants.otp_Message;

          await _mailService.SendEmailAsync(request);

            //await Task.Factory.StartNew(() =>
            //{
            //    var expiredOn = DateTime.Now.AddMinutes(3);
            //    var messageParameters = new Dictionary<string, string>
            //            {
            //                { "{OTP}",token},
            //                { "{ Expiredtime}",expiredOn.ToString()},

            //            };
            //    MessagingService.Initialize()
            //               .AddEmailService(model.Email, "Acount Verification", messageParameters, Constants.otp_Message, Constants.default_email_layout2)
            //           .Send();
            //});


            var mailrecord = new MailRecords
            {
                IsRead = false,
                Subject = "Contact",
                MessageType = MessageTypes.ContactUs,
                Receipient = model.Email,
                CreatedOn = DateTime.Now.GetDateUtcNow(),
                Content = $" customer email {model.Email} and Phone number {model.PhoneNumber}"
            };
            await _mailRecordService.AddAsync(mailrecord);

            await this.AddAsync(customerDetail);

            return model;
        }

        public async Task<tokenViewModel> AccountLogin(LoginViewModel model)
        {
            if (model.Username == null || model.Password == null)
            {
                this.Results.Add(new ValidationResult($"Email  or  Password is emptied"));
                return null;
            }

            var ValidEmail = _regexValidation.EmailValidation(model.Username);
            if (!ValidEmail)
            {
                this.Results.Add(new ValidationResult($"Invalid Email"));
                return null;
            };
            var user = await _userManager.FindByEmailAsync(model.Username);
            if (user == null)
            {
                this.Results.Add(new ValidationResult($"No user with this username {model.Username}"));
                return null;
            };

            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
            {
                this.Results.Add(new ValidationResult($"Invalid password"));
                return null;
            };

            var checkIsactive = this.SqlQuery<CustomersDetailsViewModel>
          ("SELECT * FROM [CustomersDetails] i WHERE Email= @Email AND IsDeleted <>1",
       new
       {
           Email = model.Username

       }).FirstOrDefault();


            if (checkIsactive == null)
            {
                this.Results.Add(new ValidationResult($"Account with this {model.Username} not found "));
                return null;
            }

            if (!checkIsactive.IsActive)
            {
                this.Results.Add(new ValidationResult($"Account with this {model.Username} is not Active"));
                return null;
            }

            var claims = new[]
            {
                new Claim ("Email", user.Email),
                new Claim ("Phone", user.PhoneNumber),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var token = new JwtSecurityToken(

                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(10),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)

                );
            string tokenstring = new JwtSecurityTokenHandler().WriteToken(token);
            var logResponce = new tokenViewModel();
            logResponce.Token = tokenstring;
            logResponce.ExpireOn = token.ValidTo;
            return logResponce;
            //  throw new NotImplementedException();
        }

        public async Task<CustomersDetailsViewModel> accountVerificationAsync(AccountVerificationViewModel model)
        {
            if (model == null)
            {
                this.Results.Add(new ValidationResult($"Invalid model"));
                return null;
            }

            var user = this.SqlQuery<CustomersDetails>
                ("SELECT * FROM [CustomersDetails] i WHERE Email= @Email AND IsDeleted <>1",
             new
             {
                 Email = model.Email

             }).FirstOrDefault();

            if (user == null)
            {
                this.Results.Add(new ValidationResult($" account with this Email  {model.Email} not found"));
                return null;
            }

            if (user.IsActive)
            {
                this.Results.Add(new ValidationResult($" account already Activate "));
                return null;
            }

            var tokenList = _cacheService.GetCacheData(model.Email);

            if (tokenList == null)
            {
                this.Results.Add(new ValidationResult($" OTP expired"));
                return null;
            }

            if (tokenList.Token != model.OTP )
            {
                this.Results.Add(new ValidationResult($" Incorrect OTP {model.OTP}"));
                return null;
            }

            if (tokenList.Email != model.Email)
            {
                this.Results.Add(new ValidationResult($" Incorrect Email {model.Email}"));
                return null;
            }

            user.IsActive = true;
            await this.UpdateAsync(user);

            var userDetail = (CustomersDetailsViewModel)user;
            return userDetail;
        }


    }
}
