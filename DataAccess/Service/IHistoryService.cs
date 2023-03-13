using BussinessObject.DTO;
using BussinessObject.Models;
using System;
using System.Collections.Generic;

namespace DataAccess.Service
{
    public interface IHistoryService
    {
        History AddNewHistory(long residentId, long roomId, DateTime startDate);
        public List<InvoiceDTO> CheckInvoiceToCheckOutDateForResident(long residentId, long managerId, long roomId, DateTime checkoutDate);
        List<InvoiceDTO> UpdateCheckOutDateForResident(long residentId, long roomId, long managerId, DateTime checkoutDate);

    }
}
