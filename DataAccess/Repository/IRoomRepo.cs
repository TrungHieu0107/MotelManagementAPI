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
        public List<Room> GetRoomsForCreatingInvoicesByHistoriesAndDate(List<History> histories, DateTime dateTime);
        public Room UpdateStatusWhenBookingById(long managerId, long roomId);
        public Room UpdateBookedRoomToActive(long roomId);
    }
}
