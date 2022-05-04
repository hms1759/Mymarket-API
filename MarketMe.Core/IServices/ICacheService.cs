using MarketMe.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Core.IServices
{
    public interface ICacheService
    {
        void SetCacheData(tokenViewModel model);
        tokenViewModel GetCacheData(string email);

    }
}
