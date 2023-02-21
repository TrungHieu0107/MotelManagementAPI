using BussinessObject.DTO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MotelManagerWebApp.Service.impl
{
    public class AccountApiClient : IAccountApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public AccountApiClient(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }
        public async Task<string> Authenticate(LoginDTO loginDTO)
        {
            var client = _httpClientFactory.CreateClient();
            string token = null;
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(loginDTO);
            var httpContent = new StringContent(json, encoding: System.Text.Encoding.UTF8, "application/json");
            client.BaseAddress = new System.Uri("http://localhost:5001");
            // Create an HttpClient instance with SSL/TLS enabled
            var responser = await client.PostAsync("/authenticate", httpContent);
            if (responser.IsSuccessStatusCode)
            {
                token = await responser.Content.ReadAsStringAsync();
            }

            return token;
        }


    }
}
