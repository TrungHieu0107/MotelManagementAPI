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
            var result = _context.Histories.Where(x => x.Id == id).Select(history => new HistoryDTO()
            {
                Id = id,
                RoomId = history.RoomId,
                EndDate = history.EndDate, 
                StartDate = history.StartDate,
                ResidentId = history.ResidentId
            }).FirstOrDefault();

            return result;
        }

        public HistoryDTO Update(HistoryDTO history)
        {
            var old = _context.Histories.Find(history.Id);
            if (old == null)
            {
                throw new Exception("Not found history " + history.Id);
            }
            old.RoomId = history.RoomId;
            _context.Entry(old).State = EntityState.Modified;
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

        public History FindByRoomIdForCurrentActiveRoomForManager(long roomId)
        {
            return _context.Histories.Include(h => h.Resident).Include(h => h.Room).
                FirstOrDefault(h => (h.Room.Status == RoomStatus.ACTIVE || h.Room.Status == RoomStatus.EMPTY || h.Room.Status == RoomStatus.BOOKED) && 
                h.RoomId == roomId && 
                (h.EndDate == null || h.EndDate >= DateTime.Now));
        }

        public List<History> FindByResidentId(long residentId)
        {
            return _context.Histories.Include(h => h.Room).
                            Where(h => h.ResidentId == residentId).ToList();
        }

        public bool CheckEmptyRoom(long roomId)
        {
            var historyLatest = _context.Histories
                            .Where(history => history.RoomId == roomId)
                            .OrderByDescending(history => history.Id)
                            .FirstOrDefault();
        
            if(historyLatest.EndDate == null || historyLatest.EndDate >= DateTime.Now)
            {
                return false;
            }
            
            return true;
        }

        public HistoryDTO FindByRoomId(long roomId)
        {
            throw new NotImplementedException();
        }

        public HistoryDTO UpdateCheckoutDateForResident(long residentId, long managerId, long roomId, DateTime checkoutDate)
        {
            var history = _context.Histories
                .Include(history => history.Resident)
                .Include(history => history.Room)
                .Include(history => history.Room.MotelChain)
                .Where(history => history.Resident.Status == AccountStatus.ACTIVE
                && history.Room.Status == RoomStatus.ACTIVE
                && history.ResidentId == residentId
                && history.RoomId == roomId)
                .OrderByDescending(history => history.StartDate)
                .FirstOrDefault();
            
            if(history.Room.MotelChain.ManagerId != managerId) throw new Exception("Phòng với ID: " + history.RoomId + " không tồn tại hoặc không thuộc kiểm soát của quản lý.");

            if (history == null)
            {
                return null;
            }

            history.EndDate = checkoutDate;
            _context.Entry(history).State = EntityState.Modified;
            int countUpdate = _context.SaveChanges();
            if(countUpdate <= 0)
            {
                return null;
            }

            return new HistoryDTO()
            {
                Id = history.Id,
                EndDate = history.EndDate,
                StartDate = history.StartDate,
                RoomId = history.RoomId,
                ResidentId = history.ResidentId,
                Room = new RoomDTO()
                {
                    RentFee = history.Room.RentFee,
                    FeeAppliedDate = history.Room.FeeAppliedDate,
                    Code = history.Room.Code,
                    Id = history.Room.Id,
                },
                Resident = new ResidentDTO()
                {
                    Id = history.Resident.Id,
                    FullName = history.Resident.FullName,
                    Phone = history.Resident.Phone,
                    Status = Enum.GetName(typeof(AccountStatus), history.Resident.Status),
                }
            };
            
        }

        public HistoryDTO GetLatestHistoryOfRoom(long id)
        {
            return _context.Histories
                .Where(h => h.Id == id)
                .OrderByDescending(h => h.EndDate)
                .Select(h => new HistoryDTO()
                {
                    Id = h.Id,
                    EndDate = h.EndDate,
                    StartDate= h.StartDate,
                    ResidentId=h.Resident.Id, 
                    RoomId=h.Room.Id,
                }).FirstOrDefault();
        }
    }
}
