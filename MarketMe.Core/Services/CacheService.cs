using MarketMe.Core.IServices;
using MarketMe.Core.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Core.Services
{
    public class CacheService : ICacheService
    {
        private static IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public tokenViewModel GetCacheData(string email)
        {
            var result = _cache.Get<tokenViewModel>(email);
            return result;
        }

        public void SetCacheData(tokenViewModel model)
        {
            var cacheExpiredOn = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = model.ExpireOn
            };
            _cache.Set(model.Email, model, cacheExpiredOn);
            return;
        }

        public void SetCacheData(string email, DateTime value, string token)
        {
            throw new NotImplementedException();
        }
    }
}


//public static void Add(Guid businessId, DateTime value, SubscriptionViewModel model)
//{
//    var cacheExpiredOn = new MemoryCacheEntryOptions()
//    {
//        AbsoluteExpiration = value
//    };
//    _memoryCache.Set(businessId, model, cacheExpiredOn);

//}

//public static SubscriptionViewModel Get(Guid businessId)
//{
//    var result = _memoryCache.Get(businessId);
//    return (SubscriptionViewModel)result;
//}
//    }