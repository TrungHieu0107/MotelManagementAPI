using BussinessObject.Models;
using BussinessObject.Status;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IRoomRepo
    {
        Room findRoomByCodeAndStatus(string code, RoomStatus status);
        void UpdateRoomStatus(Room room);
    }

}
