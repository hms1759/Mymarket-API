using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Core.IServices
{
    public interface ICacheService
    {
        void SetCacheData(string email, string  token);
        string GetCacheData(string email);

    }
}
