using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BussinessObject.DTO
{
    public class ElectricityCostDTO
    {
        public long Id { get; set; }
        public long Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime AppliedDate { get; set; }
        public List<InvoiceDTO> Invoices { get; set; }
    }
}
