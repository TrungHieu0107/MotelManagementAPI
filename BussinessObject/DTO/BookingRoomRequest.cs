using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.DTO
{
    public class BookingRoomRequest
    {
        public string IdentityCardNumber { get; set; }
        public long RoomId { get; set; }
        public DateTime StartDate { get; set; }
    }
}
