using BussinessObject.Models;
using BussinessObject.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IRoomRepo
    {
        Task<Room> findRoomByCodeAndStatus(string code, RoomStatus status);
        void UpdateRoomStatus(Room room);
    }
      
}
