using BussinessObject.Status;
using System.ComponentModel.DataAnnotations;

namespace BussinessObject.DTO
{
    public class AccountDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên đăng nhập phải từ 8 đến 40 kí tự")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Tên đăng nhập phải từ 8 đến 40 kí tự")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Tên đăng nhập không được chứa khoảng trắng")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu phải từ 8 đến 40 kí tự")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Mật khẩu phải từ 8 đến 40 kí tự")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Mật khẩu không được chứa khoảng trắng")]
        public string Password { get; set; }



        [Required(AllowEmptyStrings = false, ErrorMessage = "Số CMND phải 12 số")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Số CMND phải 12 số")]
        [RegularExpression(@"^([0-9]{12})$", ErrorMessage = "Sai định dạng số CMND.")]
        public string IdentityCardNumber { get; set; }


        [DataType(DataType.PhoneNumber, ErrorMessage = "Sai định dạng số điện thoại")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại phải có đủ 10 số")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải có đủ 10 số")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Vui lòng nhập đúng định dạng số điện thoại")]

        public string Phone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Họ và tên phải từ 8 đến 60 kí tự")]
        [StringLength(60, MinimumLength = 8, ErrorMessage = "Họ và tên phải từ 8 đến 60 kí tự!")]
        public string FullName { get; set; }

        public AccountStatus Status { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu xác nhận không được để trống.")]
        [Compare(nameof(Password), ErrorMessage = "Mật khẩu xác nhận không trùng khớp.")]
        public string ConfirmPassword { get; set; }

    }
}
