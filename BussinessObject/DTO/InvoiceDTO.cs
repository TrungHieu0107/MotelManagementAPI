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
        long Id { get; set; }
        int ElectricityConsumptionStart { get; set; }
        int ElectricityConsumptionEnd { get; set; }
        long ElectricityCostId { get; set; }
        int WaterConsumptionStart { get; set; }
        int WaterConsumptionEnd { get; set; }
        long WaterCostId { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime PaidDate { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        DateTime ExpiredDate { get; set; }
        long RoomId { get; set; }
        long ResidentId { get; set; }
        InvoiceStatus Status { get; set; }
        ResidentDTO Resident { get; set; }
        WaterCostDTO WaterCost { get; set; }
        ElectricityCostDTO ElectricityCost { get; set; }
        RoomDTO Room { get; set; }
    }
}
