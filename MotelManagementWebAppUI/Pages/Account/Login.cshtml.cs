using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BussinessObject.DTO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace MotelManagementWebAppUI.Pages.Login
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public LoginModel(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            this._configuration = configuration;
        }

        [BindProperty]
        public LoginDTO LoginDTO { get; set; }

        public IActionResult OnGet()
        {
            if(HttpContext.Request.Cookies["token"] != null)
            {
                return RedirectToPage("../Room/RoomList");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            string token = null;
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(LoginDTO);
            var httpContent = new StringContent(json, encoding: System.Text.Encoding.UTF8, "application/json");
            _httpClient.BaseAddress = new System.Uri("http://localhost:5001");
            // Create an HttpClient instance with SSL/TLS enabled
            var responser = await _httpClient.PostAsync("/authenticate", httpContent);
            

            if (responser.IsSuccessStatusCode)
            {
                token = await responser.Content.ReadAsStringAsync();


                var userPrincipal = ValidateToken(token);
                var authenticationProperties = new AuthenticationProperties
                {
                    ExpiresUtc = System.DateTimeOffset.UtcNow.AddMinutes(60)
                };
                await HttpContext.SignInAsync(
                 CookieAuthenticationDefaults.AuthenticationScheme,
                 userPrincipal,
                 authenticationProperties);
                if (User.Identity.IsAuthenticated)
                {
                    if (User.IsInRole("Resident"))
                    {
                        // Người dùng có vai trò Resident
                    }
                    else
                    {
                        // Người dùng không có vai trò Resident
                    }
                }
                else
                {
                    // Người dùng chưa đăng nhập
                }
                HttpContext.Response.Cookies.Append("token", token, new CookieOptions { Expires = DateTime.Now.AddMinutes(60) });
                //return RedirectToPage("../ElectricityCost/ElectricityCostList");
                return RedirectToPage("../Room/RoomList"); 
                
            }
            else if (responser.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(string.Empty, "Sai tên đăng nhập hoặc mật khẩu");
            }

            return Page();
        }

        public async Task<IActionResult> OnGetLogoutAsync()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("../Account/Login");
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
