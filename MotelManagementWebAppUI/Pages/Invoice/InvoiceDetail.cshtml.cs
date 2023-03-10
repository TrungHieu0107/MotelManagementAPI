using BussinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MotelManagementWebAppUI.Pages.Invoice
{
    public class InvoiceDetailModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public InvoiceDTO invoiceDTO { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            string data = TempData["InvoiceDetail"] as string;
            invoiceDTO = JsonConvert.DeserializeObject<InvoiceDTO>(data);
            return Page();
        }
    }
}
