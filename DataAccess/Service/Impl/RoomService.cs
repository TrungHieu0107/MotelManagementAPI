using BussinessObject.Data;
using BussinessObject.Models;
using BussinessObject.Status;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using BussinessObject.Status;
using DataAccess.Repository;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Service.Impl
{
    public class RoomService : IRoomService
    {

        //private readonly IRoomRepo _roomRepo;
        //private readonly IInvoiceRepo _invoiceRepo;
        //private readonly IHistoryRepo _historyRepo;
        //private readonly IResidentRepo _residentRepo;


        //public RoomService(IRoomRepo roomRepo, IInvoiceRepo invoiceRepo, IHistoryRepo historyRepo, IResidentRepo residentRepo)
        //{
        //    this._roomRepo = roomRepo;
        //    this._invoiceRepo = invoiceRepo;
        //    this._historyRepo = historyRepo;
        //    this._residentRepo = residentRepo;
        //}
        //public Room bookRoom(string code, string idCard, DateTime bookedDate)
        //{
        //    var room = CheckRoomAvailableToBook(code);
        //    Resident resident = (Resident)_residentRepo.GetResidentByIdentityCardNumberAndStatusAndUserName(idCard, "");
        //    bool checkBookedDate = validateBookedDate(bookedDate);
        //    // room phải tồn tại,
        //    // resident phải tồn tại và có trang thái là active
        //    // booked date phải trong khoản 15 ngày tính từ ngày hôm nay
        //    if (room != null && resident != null && resident.Status == AccountStatus.ACTIVE && checkBookedDate)
        //    {
        //        // ghi vào bản history
        //        History history = new History();
        //        history.StartDate = DateTime.Today;
        //        history.EndDate = bookedDate;
        //        history.RoomId = room.Id;
        //        history.ResidentId = resident.Id;
        //        room.Status = RoomStatus.BOOKED;
        //        _roomRepo.UpdateRoomStatus(room);

        //    }
        //    return room;



        //}

        //public Room CancelBookRoom(string code, string idCardNum)
        //{
        //    var room = CheckRoomAvailableToCancelBooking(code);
        //    if (room != null)
        //    {
        //        room.Status = RoomStatus.EMPTY;
        //        _roomRepo.UpdateRoomStatus(room);
        //    }
        //    return room;
        //}


        //private bool validateBookedDate(DateTime bookedDate)
        //{
        //    DateTime maxDateForBookingRoom = DateTime.Today.AddDays(15);
        //    maxDateForBookingRoom.AddMinutes(0);
        //    maxDateForBookingRoom.AddHours(0);

        //    return bookedDate.CompareTo(maxDateForBookingRoom) < 0 && bookedDate.CompareTo(DateTime.Today) > 0;
        //}

        //private Room CheckRoomAvailableToBook(string code)
        //{
        //    return _roomRepo.findRoomByCodeAndStatus(code, RoomStatus.EMPTY);
        //}
        //private Room CheckRoomAvailableToCancelBooking(string code)
        //{
        //    return _roomRepo.findRoomByCodeAndStatus(code, RoomStatus.BOOKED);
        //}

        //private bool checkResidentBookingHistoryByResidentIdCardNumber(string idCardNumber, int roomId)
        //{
        //    Resident resident = (Resident)_residentRepo.GetResidentByIdentityCardNumberAndStatusAndUserName(idCardNumber, "");
            
        //        if (resident == null)
        //        {
        //            return false;
        //        }
        //        var room = _historyRepo.checkResidentBookingHistoryByResidentId(resident.Id);
        //        if (room == null)
        //        {
        //            return false;
        //        }
        //        return true;          

        //}
    
        private readonly IRoomRepo _roomRepo;
        private readonly IHistoryRepo _historyRepo;
        public RoomService(IRoomRepo roomRepo, IHistoryRepo historyRepo)
        {
            _roomRepo = roomRepo;
            _historyRepo = historyRepo;
        }

        public RoomDTO AddNewRoom(string code, long rentFee, string feeAppliedDate, int status, long motelID)
        {
            RoomDTO newRoomDTO = GetRoom(code, rentFee, feeAppliedDate, status, motelID, -1);

            if (checkValidationRoom(newRoomDTO))
            {
                throw new Exception("Some field is error");
            }

            if (_roomRepo.GetRoomByCode(newRoomDTO.Code) != null)
            {
                throw new Exception("Code is already exist");
            }

            return _roomRepo.Insert(newRoomDTO);
        }

        private RoomDTO GetRoom(string code, long rentFee, string feeAppliedDate, int status, long motelID, long id)
        {
            RoomDTO room = new RoomDTO();
            room.Status = (RoomStatus)status;
            room.Code = code;
            DateTime date = DateTime.ParseExact(feeAppliedDate, "yyyy-MM-dd",
                                      System.Globalization.CultureInfo.InvariantCulture);
            room.FeeAppliedDate = date;
            room.MotelId = motelID;
            room.RentFee = rentFee;
            if (id > 0)
            {
                room.Id = id;
            }

            return room;
        }

        bool checkValidationRoom(RoomDTO room)
        {

            bool isError = false;

            if (room == null)
            {
                isError = true;
                return isError;
            }

            if (room.RentFee < 0)
            {
                isError = true;
            }

            if (room.MotelId == 0)
            {
                isError = true;
            }

            DateTime? date = room.FeeAppliedDate;
            if (!date.HasValue)
            {
                isError = true;
            }

            if (!(room.Status == RoomStatus.INACTIVE || room.Status == RoomStatus.ACTIVE ||
                room.Status == RoomStatus.EMPTY || room.Status == RoomStatus.DELETED))
            {
                isError = true;
            }

            return isError;
        }

        public RoomDTO UpdateRoom(RoomDTO room)
        {
            RoomDTO oldValue = _roomRepo.GetRoomById(room.Id);
            if (oldValue == null)
            {
                throw new Exception("Not found room " + room.Id);
            }

            if (!oldValue.RentFee.Equals(room.RentFee))
            {
                HistoryDTO history = _historyRepo.GetLatestHistoryByRoomId(room.Id);
                room = CopyToNewRoom(oldValue, room);
                oldValue.Status = RoomStatus.INACTIVE;
                if (_roomRepo.Update(oldValue).Status == RoomStatus.INACTIVE)
                {
                    _roomRepo.Insert(room);
                    history.RoomId = room.Id;
                    _historyRepo.Update(history);
                }
               
                return room;
            }

            return _roomRepo.Update(room);
        }

        private RoomDTO CopyToNewRoom(RoomDTO oldRoom, RoomDTO newRoom)
        {
            RoomDTO room = new RoomDTO();
            room.Status = newRoom.Status;
            room.MotelId = oldRoom.MotelId;
            room.RentFee = newRoom.RentFee;
            room.FeeAppliedDate = newRoom.FeeAppliedDate;

            return room;
        }

        public bool DeleteRoomById(long id)
        {
            return _roomRepo.DeleteRomById(id);
        }

        public List<RoomDTO> GetAllRoomHistoryWithFilter
        (
            long motelId,
            string roomCode,
            long minFee,
            long maxFee,
            List<int> status,
            string appliedDateAfter,
            ref Pagination pagination
        )
        {
            DateTime date = appliedDateAfter != null ? DateTime.ParseExact(appliedDateAfter, "yyyy-MM-dd",
                                      System.Globalization.CultureInfo.InvariantCulture) : DateTime.MinValue;

            List<RoomStatus> listStatusEnum = status.Select(x => (RoomStatus)x).ToList();

            if (minFee > maxFee)
            {
                throw new Exception("Mix fee is greater than max fee");
            }

            pagination.Total = _roomRepo.CountRoomHistoryWithFilter(
                motelId,
                roomCode,
                minFee,
                maxFee,
                listStatusEnum,
                date,
                pagination.CurrentPage,
                pagination.PageSize);

            return _roomRepo.GetAllRoomHistoryWithFilter(
                motelId,
                roomCode,
                minFee,
                maxFee,
                listStatusEnum,
                date,
                pagination.CurrentPage,
                pagination.PageSize).ToList();
        }
    }
}
