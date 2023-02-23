using BussinessObject.Data;
using BussinessObject.DTO;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        public HistoryDTO GetLatestHistoryByRoomId(long id)
        {
            var result = _context.Histories.Find(id);

            return JsonConvert.DeserializeObject<HistoryDTO>(JsonConvert.SerializeObject(result));
        }

        public HistoryDTO Update(HistoryDTO history)
        {
            var old = _context.Histories.Find(history.Id);
            if (old == null)
            {
                throw new Exception("Not found history " + history.Id);
            }
            History his = JsonConvert.DeserializeObject<History>(JsonConvert.SerializeObject(history));
            _context.Entry(his).State = EntityState.Modified;
            _context.SaveChanges();
            return history;
        }
    }
}
