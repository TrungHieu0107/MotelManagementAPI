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
        private readonly IMotelChainRepo _motelChainRepo;
        public AccountService(IAccountRepo accountRepo, IJwtService jwtService, IMotelChainRepo motelChainRepo)
        {
            this._accountRepo = accountRepo;
            this._jwtService = jwtService;
            this._motelChainRepo = motelChainRepo;
        }
        public string authenticate(LoginDTO loginDTO)
        {
            Account account = _accountRepo.FindAccountByUserNameAndPassword(loginDTO.UserName, loginDTO.Password);
            if (account == null)
            {
                return null;
            }

            string role = null;
            role = account.GetType().Name.ToString();
            if (role.Equals("Manager"))
            {
                if(_motelChainRepo.GetMotelWithManagerId(account.Id) != null)
                {
                    role = "Manager";
                }
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
