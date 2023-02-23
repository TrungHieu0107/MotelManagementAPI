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
        Room findRoomByCodeAndStatus(string code, RoomStatus status);
        void UpdateRoomStatus(Room room);
        RoomDTO GetRoomById(long roomId);
        RoomDTO GetRoomByCode(string roomCode);

        Task<IEnumerable<Room>> GetAllRoomInMotelChain(long motelChainId);
        RoomDTO Insert(RoomDTO room);
        bool DeleteRomById(long room);

        RoomDTO Update(RoomDTO room);

        IEnumerable<RoomDTO> GetAllRoomHistoryWithFilter
        (
            long motelId,
            string roomCode,
            long minFee,
            long maxFee,
            List<RoomStatus> listStatusEnum,
            DateTime appliedDateAfter,
            int page,
            int pageSize
        );

        long CountRoomHistoryWithFilter
        (
            long motelId,
            string roomCode,
            long minFee,
            long maxFee,
            List<RoomStatus> listStatusEnum,
            DateTime appliedDateAfter,
            int page,
            int pageSize
        );

    }

}
