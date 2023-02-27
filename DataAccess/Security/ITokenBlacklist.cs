using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Security
{
    public interface ITokenBlacklist
    {
        public void AddTokenToBlacklist(string token, TimeSpan expirationTime);
        public bool IsTokenBlacklisted(string token);


    }
}
