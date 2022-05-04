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
    public class DashBoardController : ControllerBase
    {


    }
}
