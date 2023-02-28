using BussinessObject.DTO;
using BussinessObject.Models;
using BussinessObject.Status;
using System;
using System.Collections.Generic;

namespace DataAccess.Repository
{
    public interface IHistoryRepo
    {
        History checkResidentBookingHistoryByResidentId(long residentId);
        HistoryDTO Update(HistoryDTO history);
        HistoryDTO GetLatestHistoryByRoomId(long id);
        public List<History> GetNullEndDateHistories(DateTime dateTime);
        public History Add(History history);
        public List<History> GetHistoriesOfBookedUpToDateRooms(DateTime dateTime);
        public List<History> GetNullEndDateHistoriesByResident(Resident resident);
        public History FindByRoomIdForCurrentActiveRoomForManager(long roomId);
        public List<History> FindByResidentId(long residentId);
        public bool CheckEmptyRoom(long roomId);
        HistoryDTO FindByRoomId(long roomId);
        HistoryDTO UpdateCheckoutDateForResident(long residentId, long roomId, DateTime checkoutDate);
    }
}
