using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.DTO
{
    public class ResidentDTO
    {
        List<HistoryDTO> Histories { get; set; }
        List<InvoiceDTO> Invoices { get; set; }
    }
}
