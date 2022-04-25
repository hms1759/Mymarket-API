using MarketMe.Controllers.Base;
using MarketMe.Core;
using MarketMe.Core.IServices;
using MarketMe.Share.Enum;
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
        private readonly IUserService _userService;

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

            await _userService.UserRegistration(model);

            if (_userService.HasError)
                return  ApiResponse(null,_userService.Errors ,ApiResponseCodes.ERROR);

            return ApiResponse(model, "User successfully created", ApiResponseCodes.OK);

        }


    }
}
