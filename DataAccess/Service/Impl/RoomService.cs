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
        public bool AutoUpdateBookedRoomsToActive(DateTime dateTime)
        {
            List<History> histories = _historyRepo.GetHistoriesOfBookedUpToDateRooms(dateTime);
            foreach (History history in histories)
            {
                _roomRepo.UpdateBookedRoomToActive(history.RoomId);
            }
            return true;
        }

        public Room UpdateStatusWhenBookingById(long managerId, long roomId, DateTime startDate)
        {
            return _roomRepo.UpdateStatusWhenBookingById(managerId, roomId, startDate);
        }

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
        private readonly IMotelChainRepo _motelChainRepo;

        public RoomService(IRoomRepo roomRepo, IHistoryRepo historyRepo, IMotelChainRepo motelChainRepo)
        {
            this._roomRepo = roomRepo;
            this._historyRepo = historyRepo;
            this._motelChainRepo = motelChainRepo;
        }

        public RoomDTO AddNewRoom(string code, long rentFee, string feeAppliedDate, int status, long userId)
        {
            var motelID = _motelChainRepo.GetMotelWithManagerId(userId)?.Id ?? -1;

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

        private bool checkValidationRoom(RoomDTO room)
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

            DateTime? date = room.FeeAppliedDate;
            if (!date.HasValue)
            {
                isError = true;
            }
            else if (date.Value <= DateTime.Now)
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

        public RoomDTO UpdateRoom(RoomDTO room, long userId)
        {

            var motelID = _motelChainRepo.GetMotelWithManagerId(userId)?.Id ?? -1;

            RoomDTO oldValue = _roomRepo.GetLatestRoomByRoomCode(room.Code);

            if (oldValue == null)
            {
                return null;
            }

            if (oldValue.FeeAppliedDate > DateTime.Now)
            {
                oldValue.FeeAppliedDate = room.FeeAppliedDate;
                oldValue.RentFee = room.RentFee;
                return _roomRepo.Update(oldValue);
            }

            if (!oldValue.RentFee.Equals(room.RentFee))
            {
                HistoryDTO history = _historyRepo.GetLatestHistoryByRoomId(room.Id);

                oldValue.Status = RoomStatus.INACTIVE;
                if (_roomRepo.Update(oldValue).Status == RoomStatus.INACTIVE)
                {
                    _roomRepo.Insert(room);
                    history.RoomId = room.Id;
                    _historyRepo.Update(history);
                }

                return room;
            }
            room.Id = oldValue.Id;
            room.Code = oldValue.Code;
            room.MotelId = oldValue.MotelId;
            return _roomRepo.Update(room);
        }

        public bool DeleteRoomById(long id)
        {
            return _roomRepo.DeleteRomById(id);
        }

        public List<RoomDTO> GetAllRoomHistoryWithFilter
        (
            string roomCode,
            long minFee,
            long maxFee,
            List<int> status,
            string appliedDateAfter,
            ref Pagination pagination,
            long userId
        )
        {
            DateTime date = appliedDateAfter != null ? DateTime.ParseExact(appliedDateAfter, "yyyy-MM-dd",
                                      System.Globalization.CultureInfo.InvariantCulture) : DateTime.MinValue;

            List<RoomStatus> listStatusEnum = status.Select(x => (RoomStatus)x).ToList();

            if (minFee > maxFee)
            {
                throw new InvalidProgramException("Mix fee is greater than max fee");
            }

            pagination.Total = _roomRepo.CountRoomHistoryWithFilter(
                roomCode,
                minFee,
                maxFee,
                listStatusEnum,
                date,
                pagination.CurrentPage,
                pagination.PageSize,
                userId);

            return _roomRepo.GetAllRoomHistoryWithFilter(
                roomCode,
                minFee,
                maxFee,
                listStatusEnum,
                date,
                pagination.CurrentPage,
                pagination.PageSize,
                userId).ToList();
        }

        public RoomDTOForDetail FindByIdForManager(long roomId, long managerId)
        {
            List<Room> rooms = _roomRepo.FindByIdForManager(roomId, managerId);
            Room roomWithCorrectRentFeeInCurrent = rooms.ElementAt(0);
            Room latestRecord = rooms.ElementAt(1);
            RoomDTOForDetail roomDTOForDetail = new RoomDTOForDetail();

            roomDTOForDetail.Code = latestRecord.Code;


            roomDTOForDetail.FeeAppliedDate = roomWithCorrectRentFeeInCurrent.FeeAppliedDate.ToString("dd/MM/yyyy");
            roomDTOForDetail.RentFee = roomWithCorrectRentFeeInCurrent.RentFee.ToString();
            if (latestRecord.RentFee == roomWithCorrectRentFeeInCurrent.RentFee)
            {
                roomDTOForDetail.NearestNextFeeAppliedDate = "-";
                roomDTOForDetail.NearestNextRentFee = "-";
            }
            else
            {
                roomDTOForDetail.NearestNextFeeAppliedDate = latestRecord.FeeAppliedDate.ToString("dd/MM/yyyy");
                roomDTOForDetail.NearestNextRentFee = latestRecord.RentFee.ToString();
            }

            roomDTOForDetail.Status = Enum.GetName(typeof(RoomStatus), latestRecord.Status);
            roomDTOForDetail.MotelChainName = latestRecord.MotelChain.Name;
            History history = _historyRepo.FindByRoomIdForCurrentActiveRoomForManager(roomId);
            if (history != null)
            {
                Resident resident = history.Resident;
                ResidentDTOForDetail residentDTOForDetail = new ResidentDTOForDetail();
                residentDTOForDetail.FullName = resident.FullName;
                residentDTOForDetail.IdentityCardNumber = resident.IdentityCardNumber;
                residentDTOForDetail.Status = Enum.GetName(typeof(AccountStatus), resident.Status);
                residentDTOForDetail.Phone = resident.Phone;

                roomDTOForDetail.StartDate = history.StartDate.ToString("dd/MM/yyyy");
                roomDTOForDetail.EndDate = history.EndDate == null ? "-" : history.EndDate.Value.ToString("dd/MM/yyyy");

                roomDTOForDetail.Resident = residentDTOForDetail;
            }
            return roomDTOForDetail;
        }


        public RoomDTOForDetail FindByIdForResident(long roomId, long residentId)
        {
            List<History> histories = _historyRepo.FindByResidentId(residentId).
                            Where(h => h.EndDate == null && h.StartDate < DateTime.Now).ToList();

            History history = histories.FirstOrDefault(h => h.RoomId == roomId);
            if (history != null)
            {
                List<Room> rooms = _roomRepo.FindByIdForResident(roomId);
                Room roomWithCorrectRentFeeInCurrent = rooms.ElementAt(0);
                Room latestRecord = rooms.ElementAt(1);
                RoomDTOForDetail roomDTOForDetail = new RoomDTOForDetail();

                roomDTOForDetail.Code = latestRecord.Code;
                roomDTOForDetail.FeeAppliedDate = roomWithCorrectRentFeeInCurrent.FeeAppliedDate.ToString("dd/MM/yyyy");
                roomDTOForDetail.RentFee = roomWithCorrectRentFeeInCurrent.RentFee.ToString();
                if (latestRecord.RentFee == roomWithCorrectRentFeeInCurrent.RentFee)
                {
                    roomDTOForDetail.NearestNextFeeAppliedDate = "-";
                    roomDTOForDetail.NearestNextRentFee = "-";
                }
                else
                {
                    roomDTOForDetail.NearestNextFeeAppliedDate = latestRecord.FeeAppliedDate.ToString("dd/MM/yyyy");
                    roomDTOForDetail.NearestNextRentFee = latestRecord.RentFee.ToString();
                }
                roomDTOForDetail.Status = Enum.GetName(typeof(RoomStatus), latestRecord.Status);
                roomDTOForDetail.MotelChainName = latestRecord.MotelChain.Name;
                roomDTOForDetail.StartDate = history.StartDate.ToString("dd/MM/yyyy");
                roomDTOForDetail.EndDate = history.EndDate == null ? "-" : history.EndDate.Value.ToString("dd/MM/yyyy");

                return roomDTOForDetail;
            }
            else throw new Exception("Room with ID: " + roomId + " doesn't exist or isn't renting by the resident.");
                        
        }

        public Room CheckBeforeBookingById(long managerId, long roomId)
        {
            return _roomRepo.CheckAndGetBeforeBookingById(managerId, roomId);
        }
    }
}
