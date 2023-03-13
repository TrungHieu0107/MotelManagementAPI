using BussinessObject.Status;
using System;
using System.ComponentModel.DataAnnotations;

namespace BussinessObject.DTO
{
    public class InvoiceDTO
    {
        public long Id { get; set; }
        [Display(Name = "Số điện bắt đầu tính hóa đơn")]
        public int ElectricityConsumptionStart { get; set; }
        [Display(Name = "Số điện kết thúc tính hóa đơn")]
        public int? ElectricityConsumptionEnd { get; set; }
        public long ElectricityCostId { get; set; }
        [Display(Name = "Số nước bắt đầu tính hóa đơn")]
        public int WaterConsumptionStart { get; set; }
        [Display(Name = "Số nước kết thúc tính hóa đơn")]
        public int? WaterConsumptionEnd { get; set; }
        public long WaterCostId { get; set; }

        [Display(Name = "Ngày tạo hóa đơn")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Ngày thanh toán")]
        public DateTime? PaidDate { get; set; }
        [Display(Name = "Ngày bắt đầu")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Ngày kết thúc")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Hạn đóng")]
        public DateTime? ExpiredDate { get; set; }
        public long RoomId { get; set; }
        public long ResidentId { get; set; }

        [Display(Name = "Trạng thái")]
        public InvoiceStatus Status { get; set; }
        public ResidentDTO Resident { get; set; }
        public WaterCostDTO WaterCost { get; set; }
        public ElectricityCostDTO ElectricityCost { get; set; }
        public RoomDTO Room { get; set; }
    }
}
