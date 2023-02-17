using BussinessObject.Data;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class HistoryDAO
    {
        private readonly Context _context;
        private readonly DbSet<History> _dbSet;

        public HistoryDAO()
        {
            _context = new Context();
            _dbSet = _context.Set<History>();
        }

        public async Task<History> GetByRoomId(long roomID)
        {
            return await (from x in _context.Histories
                          where x.RoomId == roomID
                          orderby x.StartDate descending
                          select x).FirstOrDefaultAsync();
        }

        public async Task<History> Update(History history)
        {
            var old = _context.Histories.FindAsync(history.Id);
            if(old == null)
            {
                throw new Exception("Not found history " + history.Id);
            }

            _context.Entry(history).State = EntityState.Modified;
            _context.SaveChangesAsync();
            return history;
        }
    }
}
