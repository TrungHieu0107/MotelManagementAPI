using BussinessObject.Status;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObject.Models
{
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public int ElectricityConsumptionStart { get; set; }
      
        public int? ElectricityConsumptionEnd { get; set; }

        [Required]
        public long ElectricityCostId { get; set; }
        [ForeignKey("ElectricityCostId")]

        public ElectricityCost ElectricityCost { get; set; }

        [Required]
        public int WaterConsumptionStart { get; set; }

       
        public int? WaterConsumptionEnd { get; set; }

        [Required]
        public long WaterCostId { get; set; }
        [ForeignKey("WaterCostId")]

      
        public WaterCost WaterCost { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? PaidDate { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ExpiredDate { get; set; }

        [Required]
        public long RoomId { get; set; }
        [ForeignKey("RoomId")]

       
        public Room Room { get; set; }

        [Required]
        public long ResidentId { get; set; }
        [ForeignKey("ResidentId")]

      
        public Resident Resident { get; set; }

        [Required]
        public InvoiceStatus Status { get; set; }
    }
}
