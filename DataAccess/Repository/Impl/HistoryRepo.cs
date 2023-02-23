using BussinessObject.Data;
using BussinessObject.Models;
using BussinessObject.Status;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class HistoryRepo : IHistoryRepo
    {
        private readonly Context _context;

        public HistoryRepo(Context context)
        {
            _context = context;
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
