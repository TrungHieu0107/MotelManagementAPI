using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MotelManagementWebAppUI.Pages.Room
{
    public class RoomListModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public RoomListModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<RoomDTO> ListRoomDTO { get; set; }
        public async Task OnGetAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                ("Bearer", HttpContext.Session.GetString("token"));
            var response = await _httpClient.GetAsync("http://localhost:5001/api/Room/get-rooms");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CommonResponse>(content);
            ListRoomDTO = JsonConvert.DeserializeObject<List<RoomDTO>> (JsonConvert.SerializeObject(result.Data));
        }
    }
}
