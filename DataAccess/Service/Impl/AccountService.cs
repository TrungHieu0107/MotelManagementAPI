using BussinessObject.DTO;
using BussinessObject.Models;
using DataAccess.Repository;
using DataAccess.Security;

namespace DataAccess.Service.Impl
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepo _accountRepo;
        private readonly IJwtService _jwtService;
        public AccountService(IAccountRepo accountRepo, IJwtService jwtService)
        {
            this._accountRepo = accountRepo;
            this._jwtService = jwtService;

        }
        public string authenticate(LoginDTO loginDTO)
        {
            Account account = _accountRepo.FindAccountByUserName(loginDTO.UserName);
            bool checkPassword = PasswordHasher.Verify(loginDTO.Password, account.Password);
            if (account == null || !checkPassword)
            {
                return null;
            }

            string role = null;
            role = account.GetType().Name.ToString();
            if (role.Equals("Manager"))
            {
                role = "Manager";
            }
            else if (role.Equals("Resident"))
            {
                role = "Resident";
            }
            else if (role.Equals("Admin"))
            {
                role = "Admin";
            }
            string token = _jwtService.GenerateJwtToken(account, role);
            return token;

        }
    }
}
