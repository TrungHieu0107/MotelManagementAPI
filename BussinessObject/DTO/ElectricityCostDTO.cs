using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.DTO
{
    public class ElectricityCostDTO
    {
        long Id { get; set; }
        long Price { get; set; }
        DateTime AppliedDate { get; set; }
        List<InvoiceDTO> Invoices { get; set; }
    }
}
