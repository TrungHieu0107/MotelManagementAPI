using BussinessObject.Data;
using BussinessObject.DTO;
using BussinessObject.Models;
using BussinessObject.Status;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public History Add(History history)
        {
            _context.Histories.Add(history);
            _context.SaveChanges();
            return history;
        }

        public List<History> GetHistoriesOfBookedUpToDateRooms(DateTime dateTime)
        {
            return _context.Histories.Include(h => h.Room).Where(h => h.Room.Status == RoomStatus.BOOKED && h.StartDate <= dateTime).ToList();
        }

        public List<History> GetNullEndDateHistories(DateTime dateTime)
        {
            return _context.Histories.Include(h => h.Room).Where(
                h => h.EndDate == null && h.StartDate <= dateTime).ToList();
        }

        public List<History> GetNullEndDateHistoriesByResident(Resident resident)
        {
            return _context.Histories.Include(h => h.Room).Where(h => h.ResidentId == resident.Id && h.EndDate == null).ToList();
        }
    }
}
