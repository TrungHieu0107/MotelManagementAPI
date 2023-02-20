using BussinessObject.DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MotelManagerWebApp.Service;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using System;

namespace MotelManagerWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IAccountApiClient _accountApiClient;
        private readonly IConfiguration _configuration;

        public UserController(IAccountApiClient accountApiClient, IConfiguration configuration)
        {
            _accountApiClient = accountApiClient;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
           string token = await _accountApiClient.Authenticate(loginDTO);
            if (token == null)
            {
                return RedirectToAction("Login","User");

            } else
            {
                var userPrincipal = ValidateToken(token);
                var authenticationProperties = new AuthenticationProperties
                {
                    ExpiresUtc = System.DateTimeOffset.UtcNow.AddMinutes(10)
                };
                await HttpContext.SignInAsync(
                  CookieAuthenticationDefaults.AuthenticationScheme,
                  userPrincipal,
                  authenticationProperties);

                HttpContext.Response.Cookies.Append("token", token, new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTime.Now.AddMinutes(10) });
                return RedirectToAction("Index", "Home");

            }
           
          
        }



        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;
            validationParameters.ValidAudience = _configuration["JwtSettings:Issuer"];
            validationParameters.ValidIssuer = _configuration["JwtSettings:Issuer"];


            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }
    }
}
