using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using BussinessObject.Status;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MotelManagementWebAppUI.Pages.Room
{
    [Authorize(Roles = "Manager")]
    public class BookRoomModel : PageModel
    {
        [BindProperty, DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        private readonly HttpClient _httpClient;
        public string token { get; set; } = "";
        [BindProperty]
        public RoomDTOForDetail BookedRoom { get; set; }
        [BindProperty]
        public long RoomId { get; set; }
        [BindProperty]
        public long ResidentId { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool IsEmpty { get; set; }
        [BindProperty(SupportsGet = true)]
        public ResidentDTO Resident { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool IsTheOnlyRecord { get; set; }
        [BindProperty(SupportsGet = true)]
        public string IdentityCardNumber { get; set; }
        [BindProperty(SupportsGet = true)]
        public string IdentityCardNumberForBooking { get; set; }

        public AccountDTO accountDTO { get; set; }  
        public BookRoomModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task OnGetAsync(long id)
        {
            ModelState.Clear();
            RoomId = id;
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
        }
        public async Task OnPostSearchResidentAsync()
        {
            if (IdentityCardNumber == null || string.IsNullOrEmpty(IdentityCardNumber))
            {
                ModelState.AddModelError("IdentityCardNumber", "Căn cước công dân không được để trống");
            }
            else if (IdentityCardNumber.Length != 12)
            {
                ModelState.AddModelError("IdentityCardNumber", "Sai định dạng căn cước công dân");
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                ("Bearer", HttpContext.Request.Cookies["token"]);
                string url = "http://localhost:5001/api/Resident/filter-resident?" +
                    "idCardNumber=" + IdentityCardNumber +
                    "&pageSize=1&currentPage=1";
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CommonResponse>(content);
                List<ResidentDTO> rs = JsonConvert.DeserializeObject<List<ResidentDTO>>(JsonConvert.SerializeObject(result.Data));
                if (rs.Count == 0)
                {
                    ModelState.AddModelError("IdentityCardNumber", "Không tồn tại người thuê với số căn cước công dân này");
                }
                else
                {
                    Resident = rs[0];
                    ResidentId = Resident.Id;
                    IdentityCardNumberForBooking = IdentityCardNumber;
                }
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                ("Bearer", HttpContext.Request.Cookies["token"]);
            string url2 = "http://localhost:5001/api/Room/detail?" +
                "roomId=" + RoomId;
            var response2 = await _httpClient.GetAsync(url2);
            var content2 = await response2.Content.ReadAsStringAsync();
            var result2 = JsonConvert.DeserializeObject<CommonResponse>(content2);
            BookedRoom = JsonConvert.DeserializeObject<RoomDTOForDetail>(JsonConvert.SerializeObject(result2.Data));
            IsEmpty = BookedRoom.Status.Equals(Enum.GetName(typeof(RoomStatus), RoomStatus.EMPTY)) ? true : false;
            IsTheOnlyRecord = BookedRoom.NearestNextRentFee.Equals("-");
        }
        public async Task<IActionResult> OnPostBookRoomAsync()
        {
            BookingRoomRequest bookingRoomRequest = new BookingRoomRequest();
            bookingRoomRequest.RoomId = RoomId;
            bookingRoomRequest.IdentityCardNumber = IdentityCardNumberForBooking;
            bookingRoomRequest.StartDate = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day);
            var bookingRoomRequestSerialized = JsonConvert.SerializeObject(bookingRoomRequest);
            var StringContent = new StringContent(bookingRoomRequestSerialized, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                ("Bearer", HttpContext.Request.Cookies["token"]);
            string url = "http://localhost:5001/api/Resident/book-room";
            var response = await _httpClient.PostAsync(url, StringContent);
            if(response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CommonResponse>(content);
                TempData["BookRoomErrorMessage"] = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(result.Message));
                return RedirectToPage("RoomDetail", new { id = RoomId.ToString() });
            }
            else
            {
                TempData["BookRoomSuccessMessage"] = "Đặt phòng thành công";
                return RedirectToPage("RoomDetail", new { id = RoomId.ToString() });
            }
        }
    }
}
