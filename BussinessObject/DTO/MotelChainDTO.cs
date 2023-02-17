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
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public MotelChainStatus Status { get; set; }
        public long ManagerId { get; set; }
        public ManagerDTO Manager { get; set; }
        public List<RoomDTO> Rooms { get; set; }
    }
}
