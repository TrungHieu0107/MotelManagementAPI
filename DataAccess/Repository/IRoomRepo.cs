using BussinessObject.DTO;
using BussinessObject.Models;
using BussinessObject.Status;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IRoomRepo
    {
        RoomDTO GetRoomById(long roomId);
        RoomDTO GetRoomByCode(string roomCode);
        RoomDTO Insert(RoomDTO room);
        bool DeleteRomById(long room);
        RoomDTO Update(RoomDTO room);
        IEnumerable<RoomDTO> GetAllRoomHistoryWithFilter
        (
            string roomCode,
            long minFee,
            long maxFee,
            List<RoomStatus> listStatusEnum,
            DateTime appliedDateAfter,
            int page,
            int pageSize,
            long userId
        );
        long CountRoomHistoryWithFilter
        (
            string roomCode,
            long minFee,
            long maxFee,
            List<RoomStatus> listStatusEnum,
            DateTime appliedDateAfter,
            int page,
            int pageSize,
            long userId
        );

    }

}
