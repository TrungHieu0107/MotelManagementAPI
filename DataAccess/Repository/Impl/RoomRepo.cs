using BussinessObject.Data;
using BussinessObject.DTO;
using BussinessObject.Models;
using BussinessObject.Status;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Repository
{
    public class RoomRepo : IRoomRepo
    {
        private readonly Context _context;

        public RoomRepo(Context context)
        {
            this._context = context;
        }

        bool IRoomRepo.DeleteRomById(long roomId)
        {
            var room = _context.Rooms.Find(roomId);
            if (room == null)
            {
                return false;
            }
            room.Status = RoomStatus.DELETED;
            _context.Entry(room).State = EntityState.Modified;
            _context.SaveChanges();

            return true;
        }


        public RoomDTO GetLatestRoomByRoomCode(string roomCode)
        {
            var result = _context.Rooms.Where(room => room.Code.Equals(roomCode)).Select(room => new RoomDTO()
            {
                Id = room.Id,
                Code = room.Code,
                Status = room.Status,
                FeeAppliedDate = room.FeeAppliedDate,
                RentFee = room.RentFee,
                MotelId = room.MotelId
            }).OrderByDescending(room => room.Id).FirstOrDefault();
            return result;
        }

        RoomDTO IRoomRepo.Insert(RoomDTO room)
        {
            Room r = new Room();
            r.Status = room.Status;
            r.RentFee = room.RentFee;
            r.FeeAppliedDate = room.FeeAppliedDate;
            r.MotelId = room.MotelId;
            r.Code = room.Code;

            _context.Rooms.Add(r);
            _context.SaveChanges();
            room.Id = r.Id;

            return room;
        }



        RoomDTO IRoomRepo.Update(RoomDTO room)
        {
            var value =  _context.Rooms.Where(r => 
                            r.Id == room.Id
                         &&
                            r.MotelId == room.MotelId
                            ).FirstOrDefault();

            if (value == null)
            {
                return null;
            }

            value.Status = room.Status;
            value.FeeAppliedDate = room.FeeAppliedDate;
            value.RentFee = room.RentFee;
            _context.Update(value);
            _context.SaveChanges();

            return room;
        }

        IEnumerable<RoomDTO> IRoomRepo.GetAllRoomHistoryWithFilter
        (
            string roomCode,
            long minFee,
            long maxFee,
            RoomStatus status,
            DateTime appliedDateAfter,
            int page,
            int pageSize,
            long userId
        )
        {

            var result = _context.Rooms
                .Include(room => room.MotelChain)
                .Where(room =>
                              (roomCode != null ? room.Code.Contains(roomCode) : true)
                          &&
                              (minFee > 0 ? room.RentFee >= minFee : true)
                          &&
                              (maxFee > 0 ? room.RentFee <= maxFee : true)
                          &&
                              (status >= 0 ? room.Status == status : true)
                          &&
                              (room.FeeAppliedDate >= appliedDateAfter)
                          &&
                             (room.MotelChain.ManagerId == userId)
                        )
                        .Select(x => new RoomDTO()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Status = x.Status,
                            FeeAppliedDate = x.FeeAppliedDate,
                            RentFee = x.RentFee,
                            MotelChain = new MotelChainDTO()
                            {
                                Id = x.MotelChain.Id,
                                Name = x.MotelChain.Name,
                                Address = x.MotelChain.Address
                            }
                        })
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize);

            return result;
        }

        public RoomDTO GetRoomByCode(string roomCode)
        {
            RoomDTO room = _context.Rooms
                .Where(x => x.Code.Equals(roomCode))
                .Select(x => new RoomDTO()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Status = x.Status,
                    FeeAppliedDate = x.FeeAppliedDate,
                    RentFee = x.RentFee,
                    MotelChain = new MotelChainDTO()
                    {
                        Id = x.MotelChain.Id,
                        Name = x.MotelChain.Name,
                        Address = x.MotelChain.Address
                    }
                }).FirstOrDefault();
            return room;
        }

        public long CountRoomHistoryWithFilter
        (
            string roomCode,
            long minFee,
            long maxFee,
            RoomStatus status,
            DateTime appliedDateAfter,
            int page,
            int pageSize,
            long userId
        )
        {
            return _context.Rooms
                .Include(room => room.MotelChain)
                .Where(room =>
                              (roomCode != null ? room.Code.Contains(roomCode) : true)
                          &&
                              (minFee > 0 ? room.RentFee >= minFee : true)
                          &&
                              (maxFee > 0 ? room.RentFee <= maxFee : true)
                          &&
                              (status >= 0 ? room.Status == status : true)
                          &&
                              (room.FeeAppliedDate >= appliedDateAfter)
                          &&
                             (room.MotelChain.ManagerId == userId)
                        ).Count();
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

        public Room UpdateStatusWhenBookingById(long managerId, long roomId, DateTime startDate)
        {
            Room room = _context.Rooms.Include(r => r.MotelChain).FirstOrDefault(r => r.Id == roomId && r.MotelChain.ManagerId == managerId);
            DateTime dateTimePoint = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)).AddDays(1);
            if (startDate < dateTimePoint)
            {
                room.Status = RoomStatus.ACTIVE;
            }
            else 
            { 
                room.Status = RoomStatus.BOOKED; 
            }
            var tracker = _context.Attach(room);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
            return room;
        }

        public List<Room> FindByIdForManager(long roomId, long managerId)
        {
            Room latestRecord = _context.Rooms.Include(r => r.MotelChain).
                FirstOrDefault(r => r.Id == roomId && r.MotelChain.ManagerId == managerId);
            if (latestRecord == null) throw new Exception("Room with ID: " + roomId + " doesn't exist or isn't managed by the manager.");
            Room roomWithCorrectRentFeeInCurrent = _context.Rooms.Include(r => r.MotelChain).Where(
                        r => r.Code == latestRecord.Code && r.FeeAppliedDate <= DateTime.Now &&
                        (r.Status == RoomStatus.INACTIVE || r.Status == RoomStatus.ACTIVE || r.Status == RoomStatus.BOOKED))
                    .OrderBy(r => r.FeeAppliedDate).LastOrDefault();
            List<Room> roomList = new List<Room>();
            roomList.Add(roomWithCorrectRentFeeInCurrent);
            roomList.Add(latestRecord);
            return roomList;
        }

        public List<Room> FindByIdForResident(long roomId)
        {
            Room latestRecord = _context.Rooms.Include(r => r.MotelChain).
                FirstOrDefault(r => r.Id == roomId);
            Room roomWithCorrectRentFeeInCurrent = _context.Rooms.Include(r => r.MotelChain).Where(
                        r => r.Code == latestRecord.Code && r.FeeAppliedDate <= DateTime.Now &&
                        (r.Status == RoomStatus.INACTIVE || r.Status == RoomStatus.ACTIVE || r.Status == RoomStatus.BOOKED))
                    .OrderBy(r => r.FeeAppliedDate).LastOrDefault();
            List<Room> roomList = new List<Room>();
            roomList.Add(roomWithCorrectRentFeeInCurrent);
            roomList.Add(latestRecord);
            return roomList;
        }

        public Room CheckAndGetBeforeBookingById(long managerId, long roomId)
        {
            Room room = _context.Rooms.Include(r => r.MotelChain).FirstOrDefault(r => r.Id == roomId && r.MotelChain.ManagerId == managerId);
            return room;
        }

        public RoomDTO UpdateCheckoutDateForResident(long roomId, DateTime checkOutDate)
        {
            var room = _context.Rooms.Find(roomId);

            if(room == null)
            {
                return null;
            }

            

            return null;
        }

        public RoomDTO FindById(long roomId)
        {
            return _context.Rooms.Where(room => room.Id == roomId).Select(room => new RoomDTO
            {
                Id = room.Id,
                MotelId = room.MotelId,
                Code = room.Code,
                FeeAppliedDate = room.FeeAppliedDate,
                Status = room.Status,
                RentFee = room.RentFee,
            }).FirstOrDefault();
        }

        public RoomDTO GetRoomByCodeForUpdating(long id, long managerId)
        {
            return _context.Rooms
                .Include(room => room.MotelChain)
                .Where(room => 
                room.Id.Equals(id) &&
                room.MotelChain.ManagerId.Equals(managerId)
                ).Select(room => new RoomDTO()
                {
                    Id=room.Id,
                    FeeAppliedDate = room.FeeAppliedDate,
                    Code = room.Code,
                    RentFee = room.RentFee,
                    Status = room.Status,
                }).FirstOrDefault();;
        }
    }
}
