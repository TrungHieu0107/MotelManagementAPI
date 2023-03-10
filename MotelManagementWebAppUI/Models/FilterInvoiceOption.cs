using System;
using System.ComponentModel.DataAnnotations;

namespace MotelManagementWebAppUI.Models
{
    public class FilterInvoiceOption
    {
        [Display(Name = "Mã phòng")]
        public string? RoomCode { get; set; }
        [Display(Name = "Trạng thái")]
        public int? Status { get; set; }
        public long? UserId { get; set; }
        [Display(Name ="Ngày thanh toán")]
        [DataType(DataType.Date)]
        public DateTime? PaidDate { get; set; }
        public int? PageSize { get; set; }
        public int? CurrentPage { get; set; }
        public long? Total { get; set; }

    }
}
