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
            this._historyRepo = historyRepo;

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
            var result = _context.Rooms.Find(roomId);
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
            var value =  _context.Rooms.Where(r => 
                            r.Id == room.Id
                         &&
                            r.MotelId == room.MotelId
                            ).FirstOrDefault();

            if (room == null)
            {
                return null;
            }
            _context.Update(room);
            _context.SaveChanges();

            return room;
        }

        IEnumerable<RoomDTO> IRoomRepo.GetAllRoomHistoryWithFilter
        (
            string roomCode,
            long minFee,
            long maxFee,
            List<RoomStatus> listStatusEnum,
            DateTime appliedDateAfter,
            int page,
            int pageSize,
            long userId
        )
        {

            var result = _context.Rooms.Where(room =>
                              roomCode != null ? room.Code.Contains(roomCode) : true
                          &&
                              minFee > 0 ? room.RentFee >= minFee : true
                          &&
                              maxFee > 0 ? room.RentFee <= maxFee : true
                          &&
                              listStatusEnum != null ? listStatusEnum.Contains(room.Status) : true
                          &&
                              room.FeeAppliedDate >= appliedDateAfter
                          &&
                              room.MotelChain.ManagerId == userId
                        ).Select(x => new RoomDTO()
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
            List<RoomStatus> listStatusEnum,
            DateTime appliedDateAfter,
            int page,
            int pageSize,
            long userId
        )
        {
            return _context.Rooms.Where(room =>
                        roomCode != null ? room.Code.Contains(roomCode) : true
                    &&
                        minFee > 0 ? room.RentFee >= minFee : true
                    &&
                        maxFee > 0 ? room.RentFee <= maxFee : true
                    &&
                        listStatusEnum != null ? listStatusEnum.Contains(room.Status) : true
                    &&
                        room.FeeAppliedDate >= appliedDateAfter
                    &&
                        room.MotelChain.ManagerId == userId
                        ).Count();
        }
    }
}
