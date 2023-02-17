using BussinessObject.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.DTO
{
    public class AccountDTO
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IdentityCardNumber { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public AccountStatus Status { get; set; }
    }
}
