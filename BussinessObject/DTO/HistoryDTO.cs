using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.DTO
{
    public class HistoryDTO
    {
        long Id { get; set; }
        long RoomId { get; set; }
        long ResidentId { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        RoomDTO Room { get; set; }
        ResidentDTO Resident { get; set; }
        
    }
}
