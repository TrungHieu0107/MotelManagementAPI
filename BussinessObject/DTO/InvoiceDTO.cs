using BussinessObject.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.DTO
{
    public class InvoiceDTO
    {
        public long Id { get; set; }
        public int ElectricityConsumptionStart { get; set; }
        public int ElectricityConsumptionEnd { get; set; }
        public long ElectricityCostId { get; set; }
        public int WaterConsumptionStart { get; set; }
        public int WaterConsumptionEnd { get; set; }
        public long WaterCostId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PaidDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public long RoomId { get; set; }
        public long ResidentId { get; set; }
        public InvoiceStatus Status { get; set; }
        public ResidentDTO Resident { get; set; }
        public WaterCostDTO WaterCost { get; set; }
        public ElectricityCostDTO ElectricityCost { get; set; }
        public RoomDTO Room { get; set; }
    }
}
