using BussinessObject.Data;
using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ManagerRepo : IManagerRepo
    {
        private readonly Context _context;

        public ManagerRepo(Context context)
        {
            _context = context;
        }
        public void AddManager(Manager manager)
        {
            _context.Add(manager);
            _context.SaveChanges();
        }
    }
}
