using BussinessObject.Status;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BussinessObject.DTO
{
    public class RoomDTO
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Nhập mã phòng")]
        [Display(Name ="Mã phòng")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Nhập giá phòng")]
        [Display(Name ="Giá thuê phòng")]
        public long RentFee { get; set; }

        [Required(ErrorMessage = "Nhập ngày áp dụng giá phòng")]
        [Display(Name ="Ngày áp dụng")]
        [DataType(DataType.Date)]
        public DateTime FeeAppliedDate { get; set; }
        [Display(Name ="Trạng thái")]
        public RoomStatus Status { get; set; }
        public long MotelId { get; set; }
        public MotelChainDTO MotelChain { get; set; }
        public List<HistoryDTO> Histories { get; set; }
        public List<InvoiceDTO> Invoices { get; set; }
        public HistoryDTO LatestHistory { get; set; }

        [Display(Name = "Giá thuê mới")]
        [Range(1, 1000000000)]
        public long? NearestNextRentFee { get; set; }

        [Display(Name = "Ngày áp dụng giá mới")]
        [DataType(DataType.Date)]
        public DateTime? NearestNextFeeAppliedDate { get; set; }
    }
}
