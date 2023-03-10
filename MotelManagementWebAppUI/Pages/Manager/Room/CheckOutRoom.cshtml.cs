﻿using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MotelManagementWebAppUI.Pages.Room
{
    public class CheckOutRoomModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public string token { get; set; } = "";
        [BindProperty(SupportsGet = true)]
        public RoomDTOForDetail Room { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<InvoiceDTO> InvoiceDTOs { get; set; }
        [BindProperty(SupportsGet = true)]
        public InvoiceDTO OldInvoice { get; set; }
        public long OldTotalElectricityCost;
        public long OldTotalWaterCost;
        public long OldTotal;
        public long OldTotalRentFee;

        [BindProperty(SupportsGet = true)]
        public InvoiceDTO NewInvoice { get; set; }
        public long NewTotalElectricityCost;
        public long NewTotalWaterCost;
        public long NewTotal;
        public long NewTotalRentFee;

        public long Total;

        [BindProperty(SupportsGet = true)] 
        public long ResidentId { get; set; }
        [BindProperty(SupportsGet = true)]
        public long RoomId { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime CheckOutDate { get; set; }
        public CheckOutRoomModel(HttpClient httpClient)
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
            Room = JsonConvert.DeserializeObject<RoomDTOForDetail>(JsonConvert.SerializeObject(result.Data));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
               ("Bearer", HttpContext.Request.Cookies["token"]);
            ResidentId = Room.Resident.Id;
            RoomId = Room.Id;
            CheckOutDate = DateTime.Now;
            string url2 = "http://localhost:5001/api/History/check-invoice-to-check-out-date?" +
                "residentId=" + ResidentId +
                "&roomId=" + RoomId +
                "&checkoutDate=" + CheckOutDate;
            var response2 = await _httpClient.GetAsync(url2);
            var content2 = await response2.Content.ReadAsStringAsync();
            var result2 = JsonConvert.DeserializeObject<CommonResponse>(content2);
            InvoiceDTOs = JsonConvert.DeserializeObject<List<InvoiceDTO>>(JsonConvert.SerializeObject(result2.Data));
            if (InvoiceDTOs.Count == 1)
            {
                NewInvoice = InvoiceDTOs[0];
            }
            else if (InvoiceDTOs.Count == 2)
            {
                OldInvoice = InvoiceDTOs[0];
                OldTotalElectricityCost = ((long)(OldInvoice.ElectricityConsumptionEnd - OldInvoice.ElectricityConsumptionStart)) * OldInvoice.ElectricityCost.Price;
                OldTotalWaterCost = ((long)(OldInvoice.WaterConsumptionEnd - OldInvoice.WaterConsumptionStart)) * OldInvoice.WaterCost.Price;
                OldTotal = OldTotalElectricityCost + OldTotalWaterCost + OldInvoice.Room.RentFee;
                OldTotalRentFee = (long)(OldInvoice.Room.RentFee * (OldInvoice.EndDate.Value - OldInvoice.StartDate).TotalDays);
                NewInvoice = InvoiceDTOs[1];
            }
            NewTotalElectricityCost = ((long)(NewInvoice.ElectricityConsumptionEnd - NewInvoice.ElectricityConsumptionStart)) * NewInvoice.ElectricityCost.Price;
            NewTotalWaterCost = ((long)(NewInvoice.WaterConsumptionEnd - NewInvoice.WaterConsumptionStart)) * NewInvoice.WaterCost.Price;
            NewTotal = NewTotalElectricityCost + NewTotalWaterCost + NewInvoice.Room.RentFee;
            NewTotalRentFee = (long)(NewInvoice.Room.RentFee * (NewInvoice.EndDate.Value - NewInvoice.StartDate).TotalDays);
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                ("Bearer", HttpContext.Request.Cookies["token"]);
            string url = "http://localhost:5001/api/History/update-check-out-date?" +
                "residentId=" + ResidentId +
                "&roomId=" + RoomId +
                "&checkoutDate=" + CheckOutDate;
            var response = await _httpClient.GetAsync(url);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CommonResponse>(content);
                TempData["BookRoomErrorMessage"] = JsonConvert.DeserializeObject<List<ResidentDTO>>(JsonConvert.SerializeObject(result.Message));
                return RedirectToPage("/Room/RoomDetail", new { id = RoomId.ToString() });
            }
            else
            {
                TempData["BookRoomSuccessMessage"] = "Xác nhận thanh toán hóa đơn và kết thúc thuê phòng";
                return RedirectToPage("/Room/RoomDetail", new { id = RoomId.ToString() });
            }
        }
    }
}