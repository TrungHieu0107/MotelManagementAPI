using BussinessObject.Models;
using DataAccess.Repository;
using System;

namespace DataAccess.Service.Impl
{
    public class HistoryService : IHistoryService
    {
        private readonly IHistoryRepo _historyRepo;
        public History AddNewHistory(long residentId, long roomId, DateTime startDate)
        {
            History history = new History();
            history.RoomId = roomId;
            history.StartDate = startDate;
            history.ResidentId = residentId;
            history.EndDate = null;
            return _historyRepo.Add(history);
        }
    }
}
