using BussinessObject.Status;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BussinessObject.DTO
{
    public class ResidentDTO
    {
        public long Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "The username can not be empty")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "User Name field must have minimum 8 and maximum 40 character!")]
        public string UserName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "The password can not be empty")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Password field must have minimum 8 and maximum 40 character!")]
        public string Password { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Số CCCD không được trống")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Identity Card field must have 12 character!")]
        [RegularExpression(@"^([0-9]{12})$", ErrorMessage = "Invalid Indentity Card Number.")]
        public string IdentityCardNumber { get; set; }


        [DataType(DataType.PhoneNumber, ErrorMessage = "Phone number is invalid")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "The Phone can not be empty")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone field must have 10 number!")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number.")]
        [Display(Name ="SĐT")]

        public string Phone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The full name can not be empty")]
        [StringLength(60, MinimumLength = 8, ErrorMessage = "full name field must have minimum 8 and maximum 60 character!")]
        [Display(Name = "Tên khách trọ")]
        public string FullName { get; set; }

        public string Status;

        public List<HistoryDTO> Histories { get; set; }
        public List<InvoiceDTO> Invoices { get; set; }
    }
}
