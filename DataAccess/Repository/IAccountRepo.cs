using BussinessObject.Models;

namespace DataAccess.Repository
{
    public interface IAccountRepo
    {
        Account FindAccountByUserNameAndPassword(string UserName, string Password);
        public bool IsManager(long id);
        Account FindAccountByUserName(string UserName);
    }
}
