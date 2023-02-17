using BussinessObject.Status;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class Invoice
    {
        [Key]
        public long Id { get; set; }
        public int ElectricityConsumptionStart { get; set; }
        public int? ElectricityConsumptionEnd { get; set; }
        public long ElectricityCostId { get; set; }
        [ForeignKey("ElectricityCostId")]
        public ElectricityCost ElectricityCost { get; set; }
        public int WaterConsumptionStart { get; set; }
        public int? WaterConsumptionEnd { get; set; }
        public long WaterCostId { get; set; }
        [ForeignKey("WaterCostId")]
        public WaterCost WaterCost { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public long RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room Room { get; set; }
        public long ResidentId { get; set; }
        [ForeignKey("ResidentId")]
        public Resident Resident { get; set; }
        public InvoiceStatus Status { get; set; }
    }
}
