using BussinessObject.Data;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class WaterCostRepo : IWaterCostRepo
    {
        private readonly Context _context;

        public WaterCostRepo(Context context)
        {
            _context = context;
        }

        public WaterCost GetWaterCostForCreatingInvoicesByDate(DateTime dateTime)
        {
            return _context.WaterCosts.Where(w => w.AppliedDate <= dateTime).OrderBy(w => w.AppliedDate).LastOrDefault();
        }
    }
}
