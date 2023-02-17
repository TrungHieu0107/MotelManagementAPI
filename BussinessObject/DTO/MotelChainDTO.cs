using BussinessObject.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.DTO
{
    public class MotelChainDTO
    {
        long Id { get; set; }
        string Name { get; set; }
        string Address { get; set; }
        MotelChainStatus Status { get; set; }
        long ManagerId { get; set; }
        ManagerDTO Manager { get; set; }
        List<RoomDTO> Rooms { get; set; }
    }
}
