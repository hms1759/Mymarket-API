using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Core.ViewModels
{
   public class tokenViewModel
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public DateTime ExpireOn { get; set; }
    }
}
