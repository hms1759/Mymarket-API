using MarketMe.Controllers.Base;
using MarketMe.Core;
using MarketMe.Core.Constant;
using MarketMe.Core.IServices;
using MarketMe.Core.ViewModels;
using MarketMe.Share.Constants;
using MarketMe.Share.Enum;
using MarketMe.Share.Extensions;
using MarketMe.Share.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMe.Controllers.Apis
{
    [Route("api/account")]
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMailService _mailService;

        public AccountController(IUserService userService)
        {
            _userService = userService;

        }

        [HttpPost("UserRegistration")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return base.ApiResponse(default, "Empty payload", ApiResponseCodes.INVALID_REQUEST);
            }

            var user = await _userService.UserRegistration(model);
         
            if (_userService.HasError)
                return ApiResponse(null, _userService.Errors, ApiResponseCodes.ERROR);
        
            return ApiResponse(user, "User successfully created", ApiResponseCodes.OK);

        }

        [HttpPost("accountVerification")]
        public async Task<IActionResult> accountVerification([FromBody] AccountVerificationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return base.ApiResponse(default, "Empty payload", ApiResponseCodes.INVALID_REQUEST);
            }

            var user = await _userService.accountVerificationAsync(model);

            if (_userService.HasError)
                return ApiResponse(null, _userService.Errors, ApiResponseCodes.ERROR);

            return ApiResponse(model, "User successfully created", ApiResponseCodes.OK);

        }


    }
}
