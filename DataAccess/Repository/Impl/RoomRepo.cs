using BussinessObject.Data;
using BussinessObject.DTO;
using BussinessObject.Models;
using BussinessObject.Status;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class RoomRepo : IRoomRepo
    {
        private readonly Context _context;
        private readonly IResidentRepo residentRepository;
        private IHistoryRepo _historyRepo;

        public RoomRepo(Context context, IResidentRepo residentRepo, IHistoryRepo historyRepo)
        {
            this._context = context;
            this.residentRepository = residentRepo;
            _historyRepo = historyRepo;
        }

        public  Room findRoomByCodeAndStatus(string code, RoomStatus status)
        {
            return _context.Rooms.Where(p => p.Code == code && p.Status == status).FirstOrDefault();
        }
        public async void UpdateRoomStatus(Room room)
        {
            _context.Entry(room).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        
        bool IRoomRepo.DeleteRomById(long roomId)
        {
            var room = _context.Rooms.Find(roomId);
            if (room == null)
            {
                return false;
            }

            _context.Entry(room).State = EntityState.Deleted;
            _context.SaveChanges();

            return true;
        }


        public RoomDTO GetRoomById(long roomId)
        {
            var result = (from room in _context.Rooms
                          where room.Id == roomId
                          select room).FirstOrDefault();
            return JsonConvert.DeserializeObject<RoomDTO>(JsonConvert.SerializeObject(result));
        }

        RoomDTO IRoomRepo.Insert(RoomDTO room)
        {
            Room r = JsonConvert.DeserializeObject<Room>(JsonConvert.SerializeObject(room));
            _context.Rooms.Add(r);
            _context.SaveChanges();

            return JsonConvert.DeserializeObject<RoomDTO>(JsonConvert.SerializeObject(r));
        }



        RoomDTO IRoomRepo.Update(RoomDTO room)
        {
            _context.Update(room);
            _context.SaveChanges();

            return room;
        }

        IEnumerable<RoomDTO> IRoomRepo.GetAllRoomHistoryWithFilter
        (
            long motelId,
            string roomCode,
            long minFee,
            long maxFee,
            List<RoomStatus> listStatusEnum,
            DateTime appliedDateAfter,
            int page,
            int pageSize
        )
        {
            var result = (from room in _context.Rooms
                          where
                              room.MotelId == motelId
                          &&
                              roomCode != null ? room.Code.Contains(roomCode) : true
                          &&
                              minFee > 0 ? room.RentFee >= minFee : true
                          &&
                              maxFee > 0 ? room.RentFee <= maxFee : true
                          &&
                              listStatusEnum != null ? listStatusEnum.Contains(room.Status) : true
                          &&
                              room.FeeAppliedDate >= appliedDateAfter
                          select room)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);

            return JsonConvert.DeserializeObject<IEnumerable<RoomDTO>>(JsonConvert.SerializeObject(result));
        }

        public async Task<IEnumerable<Room>> GetAllRoomInMotelChain(long motelChainId)
        {
            return await (from room in _context.Rooms
                          where room.MotelId == motelChainId
                           &&
                            room.Status == RoomStatus.ACTIVE
                          select room).ToListAsync();
        }

        public RoomDTO GetRoomByCode(string roomCode)
        {
            Room room = _context.Rooms.Where(x => x.Code.Equals(roomCode)).FirstOrDefault();
            RoomDTO result = JsonConvert.DeserializeObject<RoomDTO>(JsonConvert.SerializeObject(room));
            return result;
        }

        public long CountRoomHistoryWithFilter(long motelId, string roomCode, long minFee, long maxFee, List<RoomStatus> listStatusEnum, DateTime appliedDateAfter, int page, int pageSize)
        {
            return (from room in _context.Rooms
                          where
                              room.MotelId == motelId
                          &&
                              roomCode != null ? room.Code.Contains(roomCode) : true
                          &&
                              minFee > 0 ? room.RentFee >= minFee : true
                          &&
                              maxFee > 0 ? room.RentFee <= maxFee : true
                          &&
                              listStatusEnum != null ? listStatusEnum.Contains(room.Status) : true
                          &&
                              room.FeeAppliedDate >= appliedDateAfter
                          select room).Count();
        }
    }
}
