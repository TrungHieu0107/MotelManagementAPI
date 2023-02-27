using BussinessObject.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.DTO
{
    public class RoomDTOForDetail
    {
        public string Code { get; set; }
        public string Status { get; set; }
        public string RentFee { get; set; }
        public string FeeAppliedDate { get; set; }
        public string NearestNextRentFee { get; set; }
        public string NearestNextFeeAppliedDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string MotelChainName { get; set; }
        public ResidentDTOForDetail Resident { get; set; }
    }
}
