using BussinessObject.Data;
using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AccountRepo : IAccountRepo
    {
        private readonly Context _context;
        public AccountRepo(Context context)
        {
            this._context = context;
        }
        public Account FindAccountByUserNameAndPassword(string UserName, string Password)
        {
            return _context.Accounts.Where(
                u => u.UserName == UserName && u.Password == Password &&
                u.Status == BussinessObject.Status.AccountStatus.ACTIVE)
                .FirstOrDefault();
        }

        public bool IsManager(long id)
        {
           
            return _context.Accounts.Any(p => p.Id == id && p is Manager);
        }
    }
}
