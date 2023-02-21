using System;

namespace BussinessObject.DTO
{
    public class HistoryDTO
    {
        public long Id { get; set; }
        public long RoomId { get; set; }
        public long ResidentId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RoomDTO Room { get; set; }
        public ResidentDTO Resident { get; set; }

    }
}
