using BussinessObject.Data;
using BussinessObject.Models;
using BussinessObject.Status;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Repository
{
    public class InvoiceRepo : IInvoiceRepo
    {
        private readonly Context _context;
        public InvoiceRepo(Context context)
        {
            this._context = context;
        }
        public List<Invoice> checkLateInvoice(string idCard)
        {
            return _context.Invoices.Where(
                   p => p.Resident.IdentityCardNumber == idCard && p.Status == InvoiceStatus.LATE).ToList<Invoice>();
        }
    }
}
