using MarketMe.Controllers.Base;
using MarketMe.Core;
using MarketMe.Core.IServices;
using MarketMe.Core.ViewModels;
using MarketMe.Share.Constants;
using MarketMe.Share.Enum;
using MarketMe.Share.Extensions;
using MarketMe.Share.Models;
using MarketMe.Share.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMe.Controllers.Apis
{
    [Route("api/account")]
    public class AccountController : BaseController
    {
        private readonly IMailRecordService _mailRecordService;
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;
        private readonly INotifier _notifier;
        public AccountController(IUserService userService, IMailRecordService mailRecordService, AppSettings appSettings, INotifier notifier)
        {
            _userService = userService;
            _notifier = notifier;
            _appSettings = appSettings;
            _mailRecordService = mailRecordService;
        }

        [HttpPost("UserRegistration")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return base.ApiResponse(default, "Empty payload", ApiResponseCodes.INVALID_REQUEST);
            }

            var user = await _userService.UserRegistration(model);

            await Task.Factory.StartNew(async () =>
            {
                var body = await _notifier.ReadTemplate(MessageTypes.UserNotification);

                // var logoUrl = $"{Request.Scheme}://{Request.Host}/assets/images/Walure-logos-29.png";
                var messageToParse = new Dictionary<string, string>
                {
                    { "{PhoneNumber}", model.PhoneNumber},
                    { "{Email}", model.Email},
                };

                //// admin email notification
                //var Notifymsg = body.ParseTemplate(messageToParse);
                //await _notifier.SendManyAsync(_appSettings.SalesTeam.ToList(), _appSettings.SubNotificationcomp, Notifymsg);


                //email notification
                var message = body.ParseTemplate(messageToParse);
                await _notifier.SendAsync(model.Email, _appSettings.AdminEmail, message);


                await _mailRecordService.Create(new MailRecordViewModel()
                {
                    IsRead = false,
                    Subject = "Contact",
                    MessageType = MessageTypes.ContactUs,
                    Receipient = model.Email,
                    CreatedOn = DateTime.Now.GetDateUtcNow(),
                    Content = $" customer email {model.Email} and Phone number {model.PhoneNumber}"
                });
            });


            if (_userService.HasError)
                return ApiResponse(null, _userService.Errors, ApiResponseCodes.ERROR);

            return ApiResponse(model, "User successfully created", ApiResponseCodes.OK);

        }


    }
}
