using BussinessObject.DTO;
using BussinessObject.Models;
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
    }
}
