using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using DataAccess.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            try
            {             

                var token = _accountService.authenticate(loginDTO);
                if (token == null)
                {
                    commonResponse.Message = "UserName or Password is incorect";
                    return Unauthorized(commonResponse);

                }
                else
                {
                    return Ok(token);

                }
            } catch (Exception ex)
            {
                commonResponse.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, commonResponse);
            }
        }
    }
}
