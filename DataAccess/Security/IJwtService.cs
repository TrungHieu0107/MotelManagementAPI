using BussinessObject.Models;

namespace DataAccess.Security
{
    public interface IJwtService
    {
        string GenerateJwtToken(Account account, string role);
    }
}
