using System.Collections.Generic;

namespace BussinessObject.Models
{
    public class Resident : Account
    {
        public virtual List<History> Histories { get; set; }
        public virtual List<Invoice> Invoices { get; set; }
    }
}
