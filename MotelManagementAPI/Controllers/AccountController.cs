using BussinessObject.DTO.Common;
using BussinessObject.DTO;
using DataAccess.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MotelManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route("/authenticate")]
        public IActionResult Authenticate(LoginDTO loginDTO)
        {
            CommonResponse commonResponse = new CommonResponse();

            var token = _accountService.authenticate(loginDTO);
            if (token == null)
            {
                //commonResponse.Message = "UserName or Password is incorect";
                return Unauthorized();

            }
            else
            {
              
                return Ok(token);

            }
        }
    }
}
