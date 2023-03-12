using BussinessObject.DTO.Common;
using BussinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MotelManagementWebAppUI.Pages.ElectricityCost;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace MotelManagementWebAppUI.Pages.WaterCost
{
    [Authorize(Roles = "Admin")]
    public class WaterCostModel : PageModel
    {

        private readonly HttpClient _httpClient;

        int CURRENT_MONTH = DateTime.Today.Month;
        int CURRENT_YEAR = DateTime.Today.Year;



        [BindProperty(SupportsGet = true)]
        public List<WaterCostDTO> waterCostDTOs { get; set; }

        public WaterRequestDTO waterRequestDTO { get; set; }

        public WaterCostModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        [HttpGet]
        public async Task<IActionResult> OnGetAsync(int year = -1, int month = -1, int pageSize = 10, int currentPage = 1)
        {
            if (year == -1)

                year = CURRENT_YEAR;
            if (month == -1)

                month = CURRENT_MONTH;

            return await search(year, month, pageSize, currentPage);

        }




        [HttpGet]
        public async Task<IActionResult> search(int year, int month, int pageSize = 10, int currentPage = 1)
        {
            ViewData["year"] = year;
            ViewData["month"] = month;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
               ("Bearer", HttpContext.Request.Cookies["token"]);
            var response = await _httpClient.GetAsync($"http://localhost:5001/api/WaterCost/water-cost/{year}/{month}/{pageSize}/{currentPage}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CommonResponse>(content);
                waterCostDTOs = JsonConvert.DeserializeObject<List<WaterCostDTO>>(JsonConvert.SerializeObject(result.Data));

                var model = new WaterCostModel(_httpClient)
                {
                    waterCostDTOs = this.waterCostDTOs
                };

                return Page();
            }
            else
            {
                // return status code
                return StatusCode((int)response.StatusCode);


            }
           
        

    }
}
}
