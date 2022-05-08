using MarketMe.Controllers.Base;
using MarketMe.Core.IServices;
using MarketMe.Core.ViewModels;
using MarketMe.Share.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMe.Apis
{
    [Route("api/[controller]")]
    public class CustomerController : BaseController
    {

        private readonly ICustomersDetailsService _customersDetailsService;

        public CustomerController(ICustomersDetailsService customersDetailsService)
        {
          _customersDetailsService = customersDetailsService;
        }


        [HttpPost("AddCustomer")]
        public async Task<IActionResult> RegisterAsync([FromBody] CustomersDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return base.ApiResponse(default, "Empty payload", ApiResponseCodes.INVALID_REQUEST);
            }

            var user = await _customersDetailsService.AddCustomer_Entity(model);

            if (_customersDetailsService.HasError)
                return ApiResponse(null, _customersDetailsService.Errors, ApiResponseCodes.ERROR);

            return ApiResponse(user, "User successfully created", ApiResponseCodes.OK);

        }

        [HttpGet("Customer/{Id}")]
        public async Task<IActionResult> GetCustomerWithId(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return base.ApiResponse(default, "Empty payload", ApiResponseCodes.INVALID_REQUEST);
            }

            var user = await _customersDetailsService.CustomerwithId_Entity(id);

            if (_customersDetailsService.HasError)
                return ApiResponse(null, _customersDetailsService.Errors, ApiResponseCodes.ERROR);

            return ApiResponse(user, "User successfully created", ApiResponseCodes.OK);

        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return base.ApiResponse(default, "Empty payload", ApiResponseCodes.INVALID_REQUEST);
            }

            var user = await _customersDetailsService.GetallCustomer_Entity();

            if (_customersDetailsService.HasError)
                return ApiResponse(null, _customersDetailsService.Errors, ApiResponseCodes.ERROR);

            return ApiResponse(user, "User successfully created", ApiResponseCodes.OK);

        }
        [HttpPost("Delete/{Id}")]
        public async Task<IActionResult> DeleteCustomer(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return base.ApiResponse(default, "Empty payload", ApiResponseCodes.INVALID_REQUEST);
            }

            var user = await _customersDetailsService.DeleteCustomer_Entity(Id);

            if (_customersDetailsService.HasError)
                return ApiResponse(null, _customersDetailsService.Errors, ApiResponseCodes.ERROR);

            return ApiResponse(user, "User successfully created", ApiResponseCodes.OK);

        }
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateCustomer(CustomersDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return base.ApiResponse(default, "Empty payload", ApiResponseCodes.INVALID_REQUEST);
            }

            var user = await _customersDetailsService.UpdateCustomer_Entity(model);

            if (_customersDetailsService.HasError)
                return ApiResponse(null, _customersDetailsService.Errors, ApiResponseCodes.ERROR);

            return ApiResponse(user, "User successfully created", ApiResponseCodes.OK);

        }
    }
}
