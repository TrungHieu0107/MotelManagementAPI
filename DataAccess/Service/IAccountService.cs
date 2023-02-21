using BussinessObject.DTO;

namespace DataAccess.Service
{
    public interface IAccountService
    {
        string authenticate(LoginDTO loginDTO);
    }
}
