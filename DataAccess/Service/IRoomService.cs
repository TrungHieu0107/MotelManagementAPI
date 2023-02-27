using BussinessObject.Models;
using BussinessObject.Status;
using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DataAccess.Service
{
    public interface IRoomService
    {
        public Room UpdateStatusWhenBookingById(long managerId, long roomId, DateTime startDate);
        public bool AutoUpdateBookedRoomsToActive(DateTime dateTime);
        RoomDTO AddNewRoom(string code, long rentFee, string feeAppliedDate, int status, long userId);
        RoomDTO UpdateRoom(RoomDTO room, long userId);
        bool DeleteRoomById(long id);
        List<RoomDTO> GetAllRoomHistoryWithFilter
        (
            string roomCode,
            long minFee,
            long maxFee,
            List<int> status,
            string appliedDateAfter,
            ref Pagination pagination, 
            long userId
        );
        public RoomDTOForDetail FindByIdForManager(long roomId, long managerId);
        public Room CheckBeforeBookingById(long managerId, long roomId);
        public RoomDTOForDetail FindByIdForResident(long roomId, long residentId);
    }
}
