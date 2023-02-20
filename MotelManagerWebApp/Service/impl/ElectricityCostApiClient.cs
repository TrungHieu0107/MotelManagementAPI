using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MotelManagerWebApp.Service.impl
{
    public class ElectricityCostApiClient : IElectricityCostApiClientcs
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ElectricityCostApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            this._httpClientFactory = httpClientFactory;
            this._httpContextAccessor = httpContextAccessor;
        }
     
        public async Task<CommonResponse> getElectricityCost()
        {
            var client = _httpClientFactory.CreateClient();

            // Lấy đối tượng HttpContext từ IHttpContextAccessor
            var httpContext = _httpContextAccessor.HttpContext;

            // Lấy giá trị của cookie từ HttpContext
            var token = _httpContextAccessor.HttpContext.Request.Cookies["token"];
                //await _httpContextAccessor.HttpContext.GetTokenAsync(CookieAuthenticationDefaults.AuthenticationScheme, "access_token");

            client.BaseAddress = new Uri("http://localhost:5001");
           
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync("/api/ElectricityCost/get-current-electricity-cost");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                CommonResponse commonResponse = JsonConvert.DeserializeObject<CommonResponse>(body);
                var electricityCostDTO = commonResponse.Data;
                return commonResponse;
            }
            
            return null;
        }
    }
}
