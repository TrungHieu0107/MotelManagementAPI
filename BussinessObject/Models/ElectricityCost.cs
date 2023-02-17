using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class ElectricityCost
    {
        [Key]
        public long Id { get; set; }
        public long Price { get; set; }
        public DateTime AppliedDate { get; set; }
        public virtual List<Invoice> Invoices { get; set; }
    }
}
