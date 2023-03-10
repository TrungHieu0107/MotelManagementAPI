using BussinessObject.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.DTO
{
    public class ResidentDTOForDetail
    {
        public long Id { get; set; }
        public string IdentityCardNumber { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public string Status { get; set; }
        public List<RoomDTOForDetail> RoomDTOForDetails { get; set; }
    }
}
