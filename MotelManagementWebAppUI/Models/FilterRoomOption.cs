using BussinessObject.DTO.Common;
using BussinessObject.Status;
using System;
using System.ComponentModel.DataAnnotations;

namespace MotelManagementWebAppUI.Models
{
    public class FilterRoomOption
    {
        [Display(Name = "Mã phòng")]
        public string roomCode { get; set; }
        [Display(Name = "Giá nhỏ nhất")]
        public long? minFee { get; set; }
        [Display(Name = "Giá lớn nhất")]
        public long? maxFee { get; set; }
        [Display(Name = "Trạng thái")]
        public RoomStatus? status { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Ngày áp dụng")]
        public DateTime? appliedDateAfter { get; set; }

        public int? CurrentPage { get; set; }
        public int? PageSize { get; set; }
        public long? Total { get; set; }
    }
}
