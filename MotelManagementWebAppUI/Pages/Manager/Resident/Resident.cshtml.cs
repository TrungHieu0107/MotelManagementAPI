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


        public ResidentModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        [HttpGet]
        public async Task<IActionResult> OnGetAsync(string idCard = "", string phone = "", string Fullname = "", int status = -1, int pageSize = 10, int currentPage = 1)
        {
            char[] zeros = { '0' };
            if (!string.IsNullOrEmpty(phone))
            {
                phone = phone.TrimStart(zeros);
                phone = phone.Trim();
            }
            if (!string.IsNullOrEmpty(idCard))
            {
                idCard = idCard.Trim();
            }
            if (!string.IsNullOrEmpty(Fullname))
            {
                Fullname = Fullname.Trim();
            }


            
            return await Search(idCard, phone, Fullname, status, pageSize, currentPage);


        }




        [HttpGet]
        public async Task<IActionResult> Search(string idCardNumber, string phone, string Fullname, int status, int pageSize, int currentPage)
        {



            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
          ("Bearer", HttpContext.Request.Cookies["token"]);
            var response = await _httpClient.GetAsync($"http://localhost:5001/api/Resident/filter-resident?idCardNumber={idCardNumber}&phone={phone}&Fullname={Fullname}&status={status}&pageSize={pageSize}&currentPage={currentPage}");


            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CommonResponse>(content);
                residentDTOs = JsonConvert.DeserializeObject<List<ResidentDTO>>(JsonConvert.SerializeObject(result.Data));

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
