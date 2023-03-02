using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BussinessObject.DTO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MotelManagementWebAppUI.Pages.Login
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public LoginModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public LoginDTO LoginDTO { get; set; }

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
            }

            HttpContext.Session.SetString("token", token);

            return RedirectToPage("../Room/RoomList");
        }
    }
}
