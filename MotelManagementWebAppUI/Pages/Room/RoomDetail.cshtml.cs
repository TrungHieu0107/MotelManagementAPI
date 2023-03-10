using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using BussinessObject.Status;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MotelManagementWebAppUI.Pages.Room
{
    public class RoomDetailModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public string token { get; set; } = "";
        [BindProperty(SupportsGet = true)]
        public RoomDTOForDetail BookedRoom { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool IsEmpty { get; set; }
        [BindProperty(SupportsGet = true)]
        public long RoomId { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool IsTheOnlyRecord { get; set; }
        public RoomDetailModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task OnGetAsync(long id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                ("Bearer", HttpContext.Request.Cookies["token"]);
            string url = "http://localhost:5001/api/Room/detail?" +
                "roomId=" + id;
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CommonResponse>(content);
            BookedRoom = JsonConvert.DeserializeObject<RoomDTOForDetail>(JsonConvert.SerializeObject(result.Data));
            IsEmpty = BookedRoom.Status.Equals(Enum.GetName(typeof(RoomStatus), RoomStatus.EMPTY)) ? true : false;
            IsTheOnlyRecord = BookedRoom.NearestNextRentFee.Equals("-");
            RoomId = id;
        }
    }
}
