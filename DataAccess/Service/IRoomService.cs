using BussinessObject.Models;
using BussinessObject.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public interface IRoomService
    {
        public Room UpdateStatusWhenBookingById(long managerId, long roomId);
        public bool AutoUpdateBookedRoomsToActive(DateTime dateTime);
    }
}
