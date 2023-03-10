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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using BussinessObject.Status;

namespace MotelManagementWebAppUI.Pages.Manager.Resident
{
    [Authorize(Roles = "Manager")]
    public class ResidentModel : PageModel
    {
        private readonly HttpClient _httpClient;
        [BindProperty(SupportsGet = true)]
        public List<ResidentDTO> residentDTOs { get; set; }
        public ResidentDTO residentDTO { get; set; }
        public AccountDTO accountDTO { get; set; }
        public ResidentUpdateDTO residentUpdateDTO { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Phone { get; set; }
        [BindProperty(SupportsGet = true)]
        public string IdCard { get; set; }
        [BindProperty(SupportsGet = true)]
        public string FullName { get; set; }
        [BindProperty(SupportsGet = true)]
        public int Status { get; set; }
        [BindProperty(SupportsGet = true)]
        public Pagination ResultPagination { get; set; }
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; }

        public ResidentModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        [HttpGet]
        public async Task<IActionResult> OnGetAsync(string idCard = "", string phone = "", string fullName = "", int status = -1, int pageSize = 10, int currentPage = 1)
        {
            
            if (!string.IsNullOrEmpty(phone))
            {
               
                phone = phone.Trim();
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                idCard = idCard.Trim();
            }
            if (!string.IsNullOrEmpty(FullName))
            {
                FullName = FullName.Trim();
            }


            
            return await Search(idCard, phone, fullName, status, pageSize, currentPage);


        }




        [HttpGet]
        public async Task<IActionResult> Search(string idCard, string phone, string fullName, int status, int pageSize, int currentPage)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["token"]);
            var response = await _httpClient.GetAsync($"http://localhost:5001/api/Resident/filter-resident-with-pagination?idCardNumber={idCard}&phone={phone}&Fullname={fullName}&status={status}&pageSize={pageSize}&currentPage={currentPage}");
            IdCard = idCard;
            Phone = phone;
            FullName = fullName;
            Status = status;
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CommonResponse>(content);
                residentDTOs = JsonConvert.DeserializeObject<List<ResidentDTO>>(JsonConvert.SerializeObject(result.Data));
                ResultPagination = JsonConvert.DeserializeObject<Pagination>(JsonConvert.SerializeObject(result.Pagination));

                var model = new ResidentModel(_httpClient)
                {
                    residentDTOs = this.residentDTOs
                };
                return Page();
            }
            else
            {
                // return status code
                return StatusCode((int)response.StatusCode);
            }
        }

        public async Task OnPostPreviousAsync()
        {
            string url = $"http://localhost:5001/api/Resident/filter-resident-with-pagination?idCardNumber={IdCard}&phone={Phone}&Fullname={FullName}&status={Status}&pageSize={10}&currentPage={CurrentPage - 1}";
            await GetResident(url);
        }

        public async Task OnPostNextAsync()
        {
            string url = $"http://localhost:5001/api/Resident/filter-resident-with-pagination?idCardNumber={IdCard}&phone={Phone}&Fullname={FullName}&status={Status}&pageSize={10}&currentPage={CurrentPage + 1}";
            await GetResident(url);
        }

        private async Task GetResident(string url)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["token"]);
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CommonResponse>(content);
            residentDTOs = JsonConvert.DeserializeObject<List<ResidentDTO>>(JsonConvert.SerializeObject(result.Data));
            ResultPagination = JsonConvert.DeserializeObject<Pagination>(JsonConvert.SerializeObject(result.Pagination));
        }

        public async Task<IActionResult> OnGetEditResident(string IdentityCardNumber)
        {
            String phone = "";
            String Fullname = "";
            int status = -1;
            int pageSize = 1;
            int currentPage = 1;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
        ("Bearer", HttpContext.Request.Cookies["token"]);
            var response = await _httpClient.GetAsync($"http://localhost:5001/api/Resident/filter-resident?idCardNumber={IdentityCardNumber}&phone={phone}&Fullname={Fullname}&status={status}&pageSize={pageSize}&currentPage={currentPage}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CommonResponse>(content);
                residentDTOs = JsonConvert.DeserializeObject<List<ResidentDTO>>(JsonConvert.SerializeObject(result.Data));
                var tmp = residentDTOs.FirstOrDefault();
                if (tmp != null)
                {
                    residentUpdateDTO = new ResidentUpdateDTO
                    {
                        Id = tmp.Id,
                        FullName = tmp.FullName,
                        IdentityCardNumber = tmp.IdentityCardNumber,
                        Password = tmp.Password,
                        Phone = tmp.Phone,
                        Status = (int)Enum.Parse(typeof(AccountStatus), tmp.Status),


                };
                    return new JsonResult(new
                    {
                        success = true,
                        residentData = residentUpdateDTO
                    });

                    //return Partial("_EditResidentPartial", new ResidentModel(_httpClient)
                    //{
                    //    residentUpdateDTO = residentUpdateDTO
                    //});
                    //var model = new ResidentModel(_httpClient)
                    //{
                    //    residentDTOs = this.residentDTOs
                    //};
                  
                } else
                {
                    return Page();
                }
            }
            else
            {
                   return StatusCode((int)response.StatusCode);
            }



        }
    }
}
