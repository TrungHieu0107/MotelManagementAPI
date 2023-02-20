using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Security
{
    public interface IJwtService
    {
        string GenerateJwtToken(Account account, string role);
    }
}
