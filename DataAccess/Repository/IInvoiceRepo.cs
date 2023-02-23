using BussinessObject.Models;
using BussinessObject.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IInvoiceRepo
    {
        public Invoice GetPreviousInvoiceByRoomId(long roomId);
        public List<Invoice> AutoCheckLateInvoices(DateTime dateTime);
        public void Add(Invoice invoice);
        public Invoice FindById(long id);
        public bool AutoCloseInvoices(DateTime dateTime);
    }
}
