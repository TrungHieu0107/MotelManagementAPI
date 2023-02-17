using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class Resident : Account
    {
        public virtual List<History> Histories { get; set; }
        public virtual List<Invoice> Invoices { get; set; }
    }
}
