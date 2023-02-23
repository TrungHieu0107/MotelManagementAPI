using BussinessObject.Data;
using BussinessObject.DTO;
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

        public RoomRepo(Context context)
        {
            _context = context;
        }

        public List<Room> GetRoomsForCreatingInvoicesByHistoriesAndDate(List<History> histories, DateTime dateTime)
        {
            List<Room> roomsForCreatingInvoices = new List<Room>();
            foreach (History history in histories)
            {
                roomsForCreatingInvoices.Add(
                    _context.Rooms.Where(
                        r => r.Code == history.Room.Code && r.FeeAppliedDate <= dateTime &&
                        (r.Status == RoomStatus.INACTIVE || r.Status == RoomStatus.ACTIVE || r.Status == RoomStatus.BOOKED))
                    .OrderBy(r => r.FeeAppliedDate).LastOrDefault());
            }
            return roomsForCreatingInvoices;
        }

        public Room UpdateBookedRoomToActive(long roomId)
        {
            Room room = _context.Rooms.FirstOrDefault(r => r.Id == roomId);
            room.Status = RoomStatus.ACTIVE;
            var tracker = _context.Attach(room);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
            return room;
        }

        public Room UpdateStatusWhenBookingById(long managerId, long roomId)
        {
            Room room = _context.Rooms.Include(r => r.MotelChain).FirstOrDefault(r => r.Id == roomId && r.MotelChain.ManagerId == managerId);
            if (room == null) throw new Exception("Room with ID: " + roomId + " doesn't exist or isn't managed by the manager.");
            if(room.Status != RoomStatus.EMPTY) throw new Exception("Booked room's status must be EMPTY.");
            room.Status = RoomStatus.BOOKED;
            var tracker = _context.Attach(room);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
            return room;
        }
    }
}
