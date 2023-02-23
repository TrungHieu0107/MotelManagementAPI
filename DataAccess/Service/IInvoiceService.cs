using BussinessObject.Models;
using BussinessObject.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public interface IInvoiceService
    {
        public bool AutoCreateInvoices(DateTime dateTime);
        public Invoice GetPreviousInvoiceByRoomId(long roomId);
        public bool AutoCheckLateInvoices(DateTime dateTime);
        public Invoice AddInitialInvoice(long residentId, long roomId, DateTime startDate);
        public Invoice FindById(long id);
        public bool AutoCloseInvoices(DateTime dateTime);
    }
}
