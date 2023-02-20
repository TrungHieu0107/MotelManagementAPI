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
    public class RoomRepo : IRoomRepo
    {
        private readonly Context _context;
        private readonly IResidentRepo residentRepository;
        public RoomRepo(Context context, IResidentRepo residentRepo)
        {
            this._context = context;
            this.residentRepository = residentRepo;
        }

        public async Task<Room> findRoomByCodeAndStatus(string code, RoomStatus status)
        {
            return await _context.Rooms.Where(p => p.Code == code && p.Status == status).FirstOrDefaultAsync();
        }
        public async void UpdateRoomStatus(Room room)
        {
            _context.Entry(room).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
