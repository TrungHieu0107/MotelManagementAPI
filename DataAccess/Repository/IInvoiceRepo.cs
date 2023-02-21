using BussinessObject.Models;
using System.Collections.Generic;

namespace DataAccess.Repository
{
    public interface IInvoiceRepo
    {
        List<Invoice> checkLateInvoice(string idCard);
    }
}
