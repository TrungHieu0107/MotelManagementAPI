using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MotelManagementWebAppUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MotelManagementWebAppUI.Pages.Room
{
    public class RoomListModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public List<RoomDTO> ListRoomDTO { get; set; } = default(List<RoomDTO>);
        [BindProperty(SupportsGet = true)]
        public RoomDTO NewRoom { get; set; }
        public SelectList SelectListRoomStatus { get; set; } = new SelectList(new Dictionary<int, string>()
                                                    {
                                                        {0, "Hoạt động"},
                                                        {1, "Không hoạt động"},
                                                        {2, "Trống" },
                                                        {3, "Đã xóa" },
                                                        {4, "Có người ở" },
                                                    }, "Key", "Value");

        public List<string> ListStatus = new List<string>() { "Hoạt động",
                                                            "Không hoạt động",
                                                            "Trống",
                                                            "Đã xóa",
                                                            "Có người ở"
                                                        };
        [BindProperty(SupportsGet = true)]
        public FilterRoomOption filterRoomOption { get; set; } = default(FilterRoomOption);

        public RoomListModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ??
                    throw new ArgumentNullException(nameof(httpClientFactory));
            _httpClient = _httpClientFactory.CreateClient("MyClient");
            _httpClient.Timeout = TimeSpan.FromMinutes(5);

        }


        public void OnGet(FilterRoomOption filterRoomOption)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
              ("Bearer", HttpContext.Request.Cookies["token"]);
            string url = "http://localhost:5001/api/Room/get-rooms?" +
                "roomCode=" + filterRoomOption?.roomCode +
                "&minFee=" + filterRoomOption?.minFee +
                "&maxFee=" + filterRoomOption?.maxFee +
                "&status=" + (filterRoomOption?.status != null ? (int)filterRoomOption?.status : "")+
                "&appliedDateAfter=" + filterRoomOption?.appliedDateAfter +
                "&currentPage=1&pageSize=10";
            GetProduct(url);

        }

        public void OnPostSearch()
        {
            string url = "http://localhost:5001/api/Room/get-rooms?" +
                "roomCode=" + filterRoomOption?.roomCode +
                "&minFee=" + filterRoomOption?.minFee +
                "&maxFee=" + filterRoomOption?.maxFee +
                "&status=" + (filterRoomOption.status != null ? "" + (int)filterRoomOption.status : "") +
                "&appliedDateAfter=" + filterRoomOption?.appliedDateAfter +
                "&currentPage=1&pageSize=" + filterRoomOption?.PageSize;
            GetProduct(url);
        }


        public void OnPostPrevious()
        {
            string url = "http://localhost:5001/api/Room/get-rooms?" +
              "roomCode=" + filterRoomOption?.roomCode +
              "&minFee=" + filterRoomOption.minFee +
              "&maxFee=" + filterRoomOption?.maxFee +
              "&status=" + (filterRoomOption.status != null ? "" + (int)filterRoomOption.status : "") +
              "&appliedDateAfter=" + filterRoomOption?.appliedDateAfter +
              "&currentPage=" + (filterRoomOption?.CurrentPage - 1) +
              "&pageSize=" + filterRoomOption?.PageSize;
            GetProduct(url);
        }

        public void OnPostNext()
        {
            string url = "http://localhost:5001/api/Room/get-rooms?" +
              "roomCode=" + filterRoomOption?.roomCode +
              "&minFee=" + filterRoomOption.minFee +
              "&maxFee=" + filterRoomOption?.maxFee +
              "&status=" + (filterRoomOption.status != null ? "" + (int)filterRoomOption.status : "") +
              "&appliedDateAfter=" + filterRoomOption?.appliedDateAfter +
              "&currentPage=" + (filterRoomOption?.CurrentPage + 1) +
              "&pageSize=" + filterRoomOption?.PageSize;
            GetProduct(url);
        }

        private void GetProduct(string url)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
               ("Bearer", HttpContext.Request.Cookies["token"]);
            var response = _httpClient.GetAsync(url).GetAwaiter().GetResult();
           
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                var result = JsonConvert.DeserializeObject<CommonResponse>(content.ReadAsStringAsync().GetAwaiter().GetResult());
                filterRoomOption.CurrentPage = result.Pagination.CurrentPage;
                filterRoomOption.PageSize = result.Pagination.PageSize;
                filterRoomOption.Total = result.Pagination.Total;
                var x = JsonConvert.DeserializeObject<List<RoomDTO>>(JsonConvert.SerializeObject(result.Data));
                ListRoomDTO = x;
            }
            
        }

    }
}
