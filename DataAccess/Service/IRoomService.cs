using BussinessObject.Models;
using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DataAccess.Service
{
    public interface IRoomService
    {
       
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
    }
}
