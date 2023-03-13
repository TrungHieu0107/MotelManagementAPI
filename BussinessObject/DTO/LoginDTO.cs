using System.ComponentModel.DataAnnotations;

namespace BussinessObject.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Tên đăng nhập không được trống")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được trống")]
        public string Password { get; set; }
    }
}
