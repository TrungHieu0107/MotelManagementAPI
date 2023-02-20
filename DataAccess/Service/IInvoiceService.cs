using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public interface IInvoiceService
    {
        Invoice AddInititalInvoice(long residentId, long roomId, DateTime startDate);
    }
}
