using BussinessObject.Models;
using BussinessObject.Status;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service.Impl
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepo _roomRepo;
        private readonly IHistoryRepo _historyRepo;

        public RoomService(IRoomRepo roomRepo, IHistoryRepo historyRepo)
        {
            _roomRepo = roomRepo;
            _historyRepo = historyRepo;
        }

        public bool AutoUpdateBookedRoomsToActive(DateTime dateTime)
        {
            List<History> histories = _historyRepo.GetHistoriesOfBookedUpToDateRooms(dateTime);
            foreach (History history in histories)
            {
                _roomRepo.UpdateBookedRoomToActive(history.RoomId);
            }
            return true;
        }

        public Room UpdateStatusWhenBookingById(long managerId, long roomId)
        {
            return _roomRepo.UpdateStatusWhenBookingById(managerId, roomId);
        }
    }
}
