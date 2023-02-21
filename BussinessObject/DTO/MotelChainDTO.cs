using BussinessObject.Status;
using System.Collections.Generic;

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
