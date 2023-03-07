using BussinessObject.DTO.Common;
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
        public int? status { get; set; }
        [Display(Name = "Ngày áp dụng")]
        public DateTime? appliedDateAfter { get; set; }
        public Pagination? pagination { get; set; }
    }
}
