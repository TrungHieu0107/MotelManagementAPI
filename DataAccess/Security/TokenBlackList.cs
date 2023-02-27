using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Security
{
    public class TokenBlackList: ITokenBlacklist
    {
        private readonly IMemoryCache _cache;

        public TokenBlackList(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool IsTokenBlacklisted(string token)
        {
            if (_cache.TryGetValue(token, out _))
            {
                return true;
            }

            return false;
        }

        public void AddTokenToBlacklist(string token, TimeSpan expirationTime)
        {
            _cache.Set(token, "", expirationTime);
        }
    }
}
