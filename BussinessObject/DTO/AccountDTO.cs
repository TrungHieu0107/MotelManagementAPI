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
        long Id { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string IdentityCardNumber { get; set; }
        string Phone { get; set; }
        string FullName { get; set; }
        AccountStatus Status { get; set; }
    }
}
