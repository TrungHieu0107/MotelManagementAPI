using BussinessObject.Models;
using System;

namespace DataAccess.Service
{
    public interface IInvoiceService
    {
        Invoice AddInititalInvoice(long residentId, long roomId, DateTime startDate);
    }
}
