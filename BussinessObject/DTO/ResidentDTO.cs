using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.DTO
{
    public class ResidentDTO
    {
        public List<HistoryDTO> Histories { get; set; }
        public List<InvoiceDTO> Invoices { get; set; }
    }
}
