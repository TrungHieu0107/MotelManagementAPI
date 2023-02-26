using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using DataAccess.Security;
using DataAccess.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MotelManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenBlacklist _tokenBlacklist;

        public AccountController(IAccountService accountService, ITokenBlacklist tokenBlacklist)
        {
            _accountService = accountService;
            _tokenBlacklist = tokenBlacklist;
           
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

        [HttpDelete]
        [Route("Logout")]
        public IActionResult Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is missing");
            }

            // Kiểm tra xem token đã logout chưa
            if (_tokenBlacklist.IsTokenBlacklisted(token))
            {
                return BadRequest("Token has been blacklisted");
            }

            // Thêm token vào blacklist với thời gian sống là 1 ngày
            _tokenBlacklist.AddTokenToBlacklist(token, TimeSpan.FromDays(1));

            return Ok("Logged out successfully");

        }
    }
}
