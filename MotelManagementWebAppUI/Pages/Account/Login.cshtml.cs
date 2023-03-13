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
using BussinessObject.DTO.Common;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http.Headers;

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
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
             ("Bearer", HttpContext.Request.Cookies["token"]);
            var response = _httpClient.DeleteAsync($"http://localhost:5001/api/Account/Logout");

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (Request.Cookies["token"] != null)
            {
                Response.Cookies.Delete("token");
            }
            try
            {
                if (HttpContext.Request.Cookies["token"] != null)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(HttpContext.Request.Cookies["token"]);
                    var role = jwtToken.Claims.First(c => c.Type == "role").Value;
                    if (role.Equals("Resident"))
                    {
                        return Redirect("/resident/resident/resident-detail");
                    }
                    else if (role.Equals("Manager"))
                    {
                        return Redirect("/room/list");
                    }
                    else if (role.Equals("Admin"))
                    {
                        return Redirect("/admin/electricity/electricity-cost");
                    }
                    else
                    {
                        return Page();
                    }
                }
            }
            catch (Exception ex)
            {
                return Page();
            }
            
            return Page();
        }

        public IActionResult OnPostLogin()
        {
            if (ModelState.IsValid)
            {
                string token = null;
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(LoginDTO);
                var httpContent = new StringContent(json, encoding: System.Text.Encoding.UTF8, "application/json");
                _httpClient.BaseAddress = new System.Uri("http://localhost:5001");
                // Create an HttpClient instance with SSL/TLS enabled
                var responser = _httpClient.PostAsync("/authenticate", httpContent);

                if (responser.GetAwaiter().GetResult().StatusCode == System.Net.HttpStatusCode.OK)
                {
                    token = responser.GetAwaiter().GetResult().Content.ReadAsStringAsync().Result;

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(token);
                    int userId;

                    // Retrieve username from JWT token

                    // Retrieve role from JWT token
                    var role = jwtToken.Claims.First(c => c.Type == "role").Value;

                    var userPrincipal = ValidateToken(token);
                    var authenticationProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = System.DateTimeOffset.UtcNow.AddMinutes(60)
                    };
                    HttpContext.SignInAsync(
                     CookieAuthenticationDefaults.AuthenticationScheme,
                     userPrincipal,
                     authenticationProperties);

                    HttpContext.Response.Cookies.Append("token", token, new CookieOptions { Expires = DateTime.Now.AddMinutes(60) });

                    if (role.Equals("Resident"))
                    {
                        return Redirect("/resident/resident/resident-detail");
                    }
                    else if (role.Equals("Manager"))
                    {
                        return Redirect("/room/list");
                    }
                    else if (role.Equals("Admin"))
                    {
                        return Redirect("/admin/electricity/electricity-cost");
                    }
                    else
                    {
                        return Page();
                    }
                }
                else if (responser.GetAwaiter().GetResult().StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError(string.Empty, "Sai tên đăng nhập hoặc mật khẩu");
                }
                else if (responser.GetAwaiter().GetResult().StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var content = responser.GetAwaiter().GetResult().Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var result = JsonConvert.DeserializeObject<CommonResponse>(content);
                    ModelState.AddModelError(string.Empty, result.Message);
                }
            }
            

            return Page();
        }

        public async Task<IActionResult> OnGetLogoutAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
             ("Bearer", HttpContext.Request.Cookies["token"]);
            var response = await _httpClient.DeleteAsync($"http://localhost:5001/api/Account/Logout");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (Request.Cookies["token"] != null)
            {
                Response.Cookies.Delete("token");
            }

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
