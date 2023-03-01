using BussinessObject.DTO;
using BussinessObject.Models;
using System;

namespace DataAccess.Service
{
    public interface IHistoryService
    {
        History AddNewHistory(long residentId, long roomId, DateTime startDate);
        HistoryDTO UpdateCheckOutDateForResident(long residentId, long roomId, DateTime checkoutDate);

    }
}
