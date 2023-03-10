using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MotelManagementWebAppUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MotelManagementWebAppUI.Pages.Room
{
    public class RoomListModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty(SupportsGet = true)]
        public List<RoomDTO> ListRoomDTO { get; set; } = default(List<RoomDTO>);
        [BindProperty]
        public RoomDTO NewRoom { get; set; }
        public string token { get; set; } = "";
        [BindProperty]
        public FilterRoomOption filterRoomOption { get; set; }

        public RoomListModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
       
        public async Task OnGetAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                ("Bearer", HttpContext.Request.Cookies["token"]);
            string url = "http://localhost:5001/api/Room/get-rooms?" +
                "roomCode=" + filterRoomOption?.roomCode +
                "&minFee=" + filterRoomOption?.minFee +
                "&maxFee=" + filterRoomOption?.maxFee +
                "&status=" + filterRoomOption?.status +
                "&appliedDateAfter=" + filterRoomOption?.appliedDateAfter +
                "&currentPage=" + filterRoomOption?.pagination?.CurrentPage +
                "&pageSize=" + filterRoomOption?.pagination?.PageSize;
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CommonResponse>(content);
            ListRoomDTO = JsonConvert.DeserializeObject<List<RoomDTO>> (JsonConvert.SerializeObject(result.Data));
        }

        public async Task OnPostSearchAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                ("Bearer", HttpContext.Request.Cookies["token"]);
            string url = "http://localhost:5001/api/Room/get-rooms?" +
                "roomCode=" + filterRoomOption?.roomCode +
                "&minFee=" + filterRoomOption?.minFee +
                "&maxFee=" + filterRoomOption?.maxFee +
                "&status=" + filterRoomOption?.status +
                "&appliedDateAfter=" + filterRoomOption?.appliedDateAfter +
                "&currentPage=" + filterRoomOption?.pagination?.CurrentPage +
                "&pageSize=" + filterRoomOption?.pagination?.PageSize;
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CommonResponse>(content);
            ListRoomDTO = JsonConvert.DeserializeObject<List<RoomDTO>> (JsonConvert.SerializeObject(result.Data));
        }
    }
}
