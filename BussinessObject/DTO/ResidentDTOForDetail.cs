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
        public string IdentityCardNumber { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public string Status { get; set; }
        public List<RoomDTOForResidentDetail> RoomDTOForResidentDetails { get; set; }
    }
}
