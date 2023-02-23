using BussinessObject.Status;
using System.ComponentModel.DataAnnotations;

namespace BussinessObject.Models
{
    public class Account
    {
        [Key]
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IdentityCardNumber { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public AccountStatus Status { get; set; }
    }
}
