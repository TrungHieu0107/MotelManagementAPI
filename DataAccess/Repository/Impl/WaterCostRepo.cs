using BussinessObject.Data;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class WaterCostRepo : IWaterCostRepo
    {
        private readonly Context _context;
        private readonly DbSet<WaterCost> _waterCosts;

        public WaterCostRepo(Context context)
        {
            _context = context;
            _waterCosts = new Context().Set<WaterCost>();
        }

        public int AddWaterCost(WaterCost WaterCost)
        {
            _context.WaterCosts.Add(WaterCost);
           return _context.SaveChanges();
        }

        public async Task<WaterCost> GetCurrentWaterCost()
        {
            DateTime today = DateTime.Today;
            return await _context.WaterCosts.Where(
              p => p.AppliedDate.Month <= today.Month
              && p.AppliedDate.Year <= today.Year
              ).OrderByDescending(p => p.AppliedDate).FirstOrDefaultAsync();
        }

        public WaterCost GetWaterCostAfterDate(DateTime date)
        {
            return _context.WaterCosts.Where(p => p.AppliedDate > date).FirstOrDefault();
        }

        public async Task<IEnumerable<WaterCost>> GetWaterCostByMonthAndYear(int month, int year, int currentPage, int pageSize)
        {
            int skipCount = (currentPage - 1) * pageSize;
            int takeCount = pageSize;
            var query = _context.WaterCosts.AsQueryable();
            if (year > 0)
                query = query.Where(p => p.AppliedDate.Year == year);
            if (month > 0 && month <= 12)
                query = query.Where(p => p.AppliedDate.Month == month);
            return await query.Skip(skipCount).Take(takeCount).ToListAsync();
        }

        public int UpdateWaterCost(WaterCost WaterCost)
        {
            _context.Entry(WaterCost).State = EntityState.Modified;
           return _context.SaveChanges();
        }
       
        public async Task<WaterCost> GetWaterCostOfInvoiceById(long id)
        {
            return await _waterCosts.FindAsync(id);
        }
    }
}
