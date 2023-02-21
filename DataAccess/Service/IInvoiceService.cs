using BussinessObject.Models;
using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using System;
using System.Collections.Generic;

namespace DataAccess.Service
{
    public interface IInvoiceService
    {
        Invoice AddInititalInvoice(long residentId, long roomId, DateTime startDate);
        List<InvoiceDTO> GetInvoiceHistoryOfRoom(long roomId, ref Pagination pagination);
    }
}
