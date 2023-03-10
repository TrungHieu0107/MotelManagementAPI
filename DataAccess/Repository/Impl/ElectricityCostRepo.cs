using BussinessObject.Data;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ElectricityCostRepo : IElectricityCostRepo
    {

        private readonly Context _context;
        private readonly DbSet<ElectricityCost> _electricityCost;

        public ElectricityCostRepo(Context context)
        {
            _context = context;
            _electricityCost = _context.Set<ElectricityCost>();
        }

        public async Task<ElectricityCost> GetCurrentElectrictyCost()
        {
            DateTime today = DateTime.Today;
            return await _context.ElectricityCosts.Where(
              p => p.AppliedDate.Month <= today.Month
              && p.AppliedDate.Year <= today.Year
              ).OrderByDescending(p => p.AppliedDate).FirstOrDefaultAsync();
        }

        public int UpdateElectricityCost(ElectricityCost electricityCost)
        {
            _context.Entry(electricityCost).State = EntityState.Modified;
            return _context.SaveChanges();
        }

        public int AddElectricityCost(ElectricityCost electricityCost)
        {
            _context.ElectricityCosts.Add(electricityCost);
            return _context.SaveChanges();
        }

        public async Task<IEnumerable<ElectricityCost>> GetElectricityCostByMonthAndYear(int month, int year, int currentPage, int pageSize)
        {
            int skipCount = (currentPage - 1) * pageSize;
            int takeCount = pageSize;

            var query = _context.ElectricityCosts.AsQueryable();
            if (year > 0)
                query = query.Where(p => p.AppliedDate.Year == year);
            if (month > 0 && month <= 12)
                query = query.Where(p => p.AppliedDate.Month == month);
            return await query.Skip(skipCount).Take(takeCount).ToListAsync();
        }

        public ElectricityCost GetElectricitAfterDate(DateTime date)
        {
            return _context.ElectricityCosts.Where(p => p.AppliedDate > date).FirstOrDefault();
        }
        
        public async Task<ElectricityCost> GetElectricityCostById(long id)
        {
            return await _electricityCost.FindAsync(id);
        }
        public ElectricityCost GetElectricityCostForCreatingInvoicesByDate(DateTime dateTime)
        {
            return _context.ElectricityCosts.Where(e => e.AppliedDate <= dateTime).OrderBy(e => e.AppliedDate).LastOrDefault();
        }
    }
}
