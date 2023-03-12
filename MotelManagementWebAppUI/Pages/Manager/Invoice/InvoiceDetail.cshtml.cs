using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MotelManagementWebAppUI.Pages.Invoice
{
    public class InvoiceDetailModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
        [BindProperty(SupportsGet = true)]
        public InvoiceDTO invoiceDTO { get; set; }
        public long NewTotalElectricityCost;
        public long NewTotalWaterCost;
        public long NewTotal;
        public long NewTotalRentFee;
        public long residentId;
        public long inoviceId;

        public long Total;
        public InvoiceDetailModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ??
                    throw new ArgumentNullException(nameof(httpClientFactory));
            _httpClient = _httpClientFactory.CreateClient("MyClient");
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
            _httpClient.BaseAddress = new Uri("http://localhost:5001");
        }

        public IActionResult OnGet(long id)
        {
            string url = "http://localhost:5001/api/Invoice/get-invoice-detail?id=" + id;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
              ("Bearer", HttpContext.Request.Cookies["token"]);
            var response = _httpClient.GetAsync(url).GetAwaiter().GetResult();
            var content = response.Content;
            var result = JsonConvert.DeserializeObject<CommonResponse>(content.ReadAsStringAsync().GetAwaiter().GetResult());
            invoiceDTO = JsonConvert.DeserializeObject<InvoiceDTO>(JsonConvert.SerializeObject(result.Data));
            return Page();
        }

        public IActionResult OnGetPayInvoice(long residentId, long inoviceId)
        {
            return new JsonResult(new
            {
                success = false,
                message = "Xác nhận thanh toán không thành công",
            });
        }
    }
}
