using BussinessObject.Data;
using BussinessObject.Models;
using System.Linq;

namespace DataAccess.Repository
{
    public class HistoryRepo : IHistoryRepo
    {
        private readonly Context _context;
        public HistoryRepo(Context context) { 
            this._context = context;
        
        }    
        public History checkResidentBookingHistoryByResidentId(long residentId)
        {
            return _context.Histories.Where(p => p.ResidentId == residentId).FirstOrDefault();
        }
    }
}
