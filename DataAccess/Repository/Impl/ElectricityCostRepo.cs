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
    public class ElectricityCostRepo : IElectricityCostRepo
    {
        private readonly Context _context;

        public ElectricityCostRepo(Context context)
        {
            _context = context;
        }

        public ElectricityCost GetElectricityCostForCreatingInvoicesByDate(DateTime dateTime)
        {
            return _context.ElectricityCosts.Where(e => e.AppliedDate <= dateTime).OrderBy(e => e.AppliedDate).LastOrDefault();
        }
    }
}
