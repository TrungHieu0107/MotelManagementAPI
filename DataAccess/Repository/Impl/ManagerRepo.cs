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

        Context context;

        public ManagerRepo(Context context)
        {
            this.context = context;
        }

        public void AddManager(Manager manager)
        {
            context.Add(manager);
            context.SaveChanges();
        }
    }
}
