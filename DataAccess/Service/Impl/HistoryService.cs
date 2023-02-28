using BussinessObject.CommonConstant;
using BussinessObject.DTO;
using BussinessObject.Models;
using DataAccess.Repository;
using System;

namespace DataAccess.Service.Impl
{
    public class HistoryService : IHistoryService
    {
        private readonly IHistoryRepo _historyRepo;

        public HistoryService(IHistoryRepo historyRepo)
        {
            _historyRepo = historyRepo;
        }

        public History AddNewHistory(long residentId, long roomId, DateTime startDate)
        {
            History history = new History();
            history.RoomId = roomId;
            history.StartDate = startDate;
            history.ResidentId = residentId;
            history.EndDate = null;
            return _historyRepo.Add(history);
        }

        public HistoryDTO UpdateCheckOutDateForResident(long residentId, long roomId, DateTime checkoutDate)
        {
            DateTime minCheckOutDate = DateTime.Now.AddMonths(ApplicationResource.MIN_DISTANCE_UPDATE_CHECK_OUT);
            if (checkoutDate <= minCheckOutDate)
                return null;

            HistoryDTO result = _historyRepo.UpdateCheckoutDateForResident(residentId, roomId, checkoutDate);

            return result;  
        }
    }
}
