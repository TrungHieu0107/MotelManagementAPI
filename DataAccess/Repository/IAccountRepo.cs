using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IAccountRepo
    {
        Account FindAccountByUserNameAndPassword(string UserName, string Password);
        public bool IsManager(long id);
    }
}
