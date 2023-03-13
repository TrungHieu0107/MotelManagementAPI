using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MotelManagementWebAppUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MotelManagementWebAppUI.Pages.Resident.Invoice
{
    [Authorize(Roles = "Resident")]
    public class InvoiceListModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public List<InvoiceDTO> ListInvoice { get; set; } = default(List<InvoiceDTO>);

        [BindProperty(SupportsGet = true)]
        public FilterInvoiceOption filterInvoiceOption { get; set; } = default(FilterInvoiceOption);

        public List<String> ListInvocieStatus { get; set; } = new List<string>
        {
            "Chưa thanh toán", "Đã thanh toán", "Trễ hạn"
        };

        public SelectList SelectLisInvoiceStatus { get; set; } = new SelectList(new Dictionary<int, string>()
                                                    {
                                                        {0, "Chưa thanh toán"},
                                                        {1, "Đã thanh toán"},
                                                        {2, "Trễ hạn" },
                                                    }, "Key", "Value");

        public InvoiceListModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ??
                    throw new ArgumentNullException(nameof(httpClientFactory));
            _httpClient = _httpClientFactory.CreateClient("MyClient");
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
            _httpClient.BaseAddress = new Uri("http://localhost:5001");
        }
        public async Task OnGetAsync()
        {

            string url = "/api/Invoice/get-invoices?" +
                "roomCode=" + filterInvoiceOption?.RoomCode +
                "&status=" + filterInvoiceOption?.Status +
                "&paidDate=" + filterInvoiceOption?.PaidDate +
                "&currentPage=1&pageSize=10";
            await GetInvoice(url);
        }

        public async Task OnPostSearchAsync()
        {
            string url = "/api/Invoice/get-invoices?" +
                "roomCode=" + filterInvoiceOption?.RoomCode +
                "&status=" + filterInvoiceOption?.Status +
                "&paidDate=" + filterInvoiceOption?.PaidDate +
                "&currentPage=" + filterInvoiceOption?.CurrentPage +
                "&pageSize=" + filterInvoiceOption.PageSize;
            await GetInvoice(url);
        }

        public async Task OnPostNextAsync(long index)
        {
            string url = "/api/Invoice/get-invoices?" +
                "roomCode=" + filterInvoiceOption?.RoomCode +
                "&status=" + filterInvoiceOption?.Status +
                "&paidDate=" + filterInvoiceOption?.PaidDate +
                "&currentPage=" + (filterInvoiceOption?.CurrentPage + index) +
                "&pageSize=" + filterInvoiceOption.PageSize;
            await GetInvoice(url);
        }

        public async Task OnPostPreviousAsync(long index)
        {
            string url = "/api/Invoice/get-invoices?" +
                "roomCode=" + filterInvoiceOption?.RoomCode +
                "&status=" + filterInvoiceOption?.Status +
                "&paidDate=" + filterInvoiceOption?.PaidDate +
                "&currentPage=" + (filterInvoiceOption?.CurrentPage - index) +
                "&pageSize=" + filterInvoiceOption.PageSize;
            await GetInvoice(url);
        }

        public async Task GetInvoice(string url)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
              ("Bearer", HttpContext.Request.Cookies["token"]);
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CommonResponse>(content);
            filterInvoiceOption.CurrentPage = result.Pagination.CurrentPage;
            filterInvoiceOption.PageSize = result.Pagination.PageSize;
            filterInvoiceOption.Total = result.Pagination.Total;
            ListInvoice = JsonConvert.DeserializeObject<List<InvoiceDTO>>(JsonConvert.SerializeObject(result.Data));
        }

        public async Task<IActionResult> OnGetInvoiceDetail(long id)
        {
            string url = "http://localhost:5001/api/Invoice/get-invoice-detail?id=" + id;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
              ("Bearer", HttpContext.Request.Cookies["token"]);
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CommonResponse>(content);
            if (result != null)
            {
                if (result.Data != null)
                {
                    TempData["InvoiceDetail"] = JsonConvert.SerializeObject(result.Data);
                    return RedirectToPage("InvoiceDetail");
                }
            }
            return new JsonResult(new
            {
                success = false,
            });
        }

        public async Task<IActionResult> OnPostDetailAsync(long Id)
        {
            string url = "http://localhost:5001/api/Invoice/get-invoice-detail?id=" + Id;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
              ("Bearer", HttpContext.Request.Cookies["token"]);
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CommonResponse>(content);
            if (result != null)
            {
                if (result.Data != null)
                {
                    TempData["InvoiceDetail"] = JsonConvert.SerializeObject(result.Data);
                    return RedirectToPage("InvoiceDetail");
                }
            }
            return new JsonResult(new
            {
                success = false,
            });
        }
    }
}
