using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.DTO
{
    public class ElectricityCostDTO
    {
        public long Id { get; set; }
        public long Price { get; set; }
        public DateTime AppliedDate { get; set; }
        public List<InvoiceDTO> Invoices { get; set; }
    }
}
