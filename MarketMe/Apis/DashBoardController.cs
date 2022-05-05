using MarketMe.Controllers.Base;
using MarketMe.Core.IServices;
using MarketMe.Share.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMe.Apis
{
    [Route("api/[controller]")]
    [Authorize]
    public class DashBoardController : BaseController
    {
        private readonly ICustomersDetailsService _customersDetailsService;

        public DashBoardController(ICustomersDetailsService customersDetailsService)
        {
            _customersDetailsService = customersDetailsService;
        }

        [HttpPost("UserRegistration")]
        public async Task<IActionResult> RegisterAsync(string email)
        {
            if (!ModelState.IsValid)
            {
                return base.ApiResponse(default, "Empty payload", ApiResponseCodes.INVALID_REQUEST);
            }

            var customer = await _customersDetailsService.GetCustomerWithEmial(email);

            if (_customersDetailsService.HasError)
                return ApiResponse(null, _customersDetailsService.Errors, ApiResponseCodes.ERROR);

            return ApiResponse(customer, "", ApiResponseCodes.OK);

        }


    }
}
