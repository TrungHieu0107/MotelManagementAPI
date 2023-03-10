using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MotelManagementWebAppUI.Pages.Resident
{
    public class ResidentDetailModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public string token { get; set; } = "";
        [BindProperty(SupportsGet = true)]
        public ResidentDTOForDetail Resident { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Status { get; set; }
        [BindProperty(SupportsGet = true)]
        public long ResidentId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; }
        [BindProperty(SupportsGet = true)]
        public Pagination ResultPagination { get; set; }

        public ResidentDetailModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task OnGetAsync(long id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                ("Bearer", HttpContext.Request.Cookies["token"]);
            string url = "http://localhost:5001/api/Resident/detail?" +
                "residentId=" + id +
                "&pageSize=5" +
                "&currentPage=1" +
                "&roomStatus=RENTING";
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CommonResponse>(content);
            Status = "RENTING";
            Resident = JsonConvert.DeserializeObject<ResidentDTOForDetail>(JsonConvert.SerializeObject(result.Data));
            ResidentId = Resident.Id;
            ResultPagination = JsonConvert.DeserializeObject<Pagination>(JsonConvert.SerializeObject(result.Pagination));
        }
        public async Task OnPostSearchAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                ("Bearer", HttpContext.Request.Cookies["token"]);
            string url = "http://localhost:5001/api/Resident/detail?" +
                "residentId=" + ResidentId +
                "&pageSize=5" +
                "&currentPage=1" +
                "&roomStatus=" + Status;
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CommonResponse>(content);
            Resident = JsonConvert.DeserializeObject<ResidentDTOForDetail>(JsonConvert.SerializeObject(result.Data));
            ResultPagination = JsonConvert.DeserializeObject<Pagination>(JsonConvert.SerializeObject(result.Pagination));
        }


        public async Task OnPostPreviousAsync()
        {
            string url = "http://localhost:5001/api/Resident/detail?" +
                "residentId=" + ResidentId +
                "&pageSize=5" +
                "&currentPage=" + (CurrentPage - 1) +
                "&roomStatus=" + Status;
            await GetProduct(url);
        }

        public async Task OnPostNextAsync()
        {
            string url = "http://localhost:5001/api/Resident/detail?" +
                "residentId=" + ResidentId +
                "&pageSize=5" +
                "&currentPage=" + (CurrentPage + 1) +
                "&roomStatus=" + Status;
            await GetProduct(url);
        }

        private async Task GetProduct(string url)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
               ("Bearer", HttpContext.Request.Cookies["token"]);
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CommonResponse>(content);
            Resident = JsonConvert.DeserializeObject<ResidentDTOForDetail>(JsonConvert.SerializeObject(result.Data));
            ResultPagination = JsonConvert.DeserializeObject<Pagination>(JsonConvert.SerializeObject(result.Pagination));
        }
    }
}
