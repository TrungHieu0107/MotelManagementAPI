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
        private readonly IRoomRepo _roomRepo;
        private readonly IHistoryRepo _historyRepo;
        private readonly IMotelChainRepo _motelChainRepo;
        private readonly IInvoiceRepo _invoiceRepo;

        public RoomService(IRoomRepo roomRepo, IHistoryRepo historyRepo, IMotelChainRepo motelChainRepo, IInvoiceRepo invoiceRepo)
        {
            _roomRepo = roomRepo;
            _historyRepo = historyRepo;
            _motelChainRepo = motelChainRepo;
            _invoiceRepo = invoiceRepo;
        }

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
                    _invoiceRepo.UpdateRoomIdfOfInvoice(room.Id, oldValue.Id);
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
            bool isEmpty = _historyRepo.CheckEmptyRoom(id);
            if (isEmpty){
                return _roomRepo.DeleteRomById(id);
            }

            throw new TaskCanceledException("Room has already rented");
        }

        public List<RoomDTO> GetAllRoomHistoryWithFilter
        (
            string roomCode,
            long minFee,
            long maxFee,
            int status,
            string appliedDateAfter,
            ref Pagination pagination,
            long userId
        )
        {
            DateTime date = appliedDateAfter != null ? DateTime.ParseExact(appliedDateAfter, "yyyy-MM-dd",
                                      System.Globalization.CultureInfo.InvariantCulture) : DateTime.MinValue;

            if (minFee > maxFee)
            {
                throw new InvalidProgramException("Mix fee is greater than max fee");
            }

            pagination.Total = _roomRepo.CountRoomHistoryWithFilter(
                roomCode,
                minFee,
                maxFee,
                (RoomStatus) status,
                date,
                pagination.CurrentPage,
                pagination.PageSize,
                userId);

            var listRoom = _roomRepo.GetAllRoomHistoryWithFilter(
                roomCode,
                minFee,
                maxFee,
                (RoomStatus)status,
                date,
                pagination.CurrentPage,
                pagination.PageSize,
                userId).ToList();

            listRoom.ForEach(r =>
            {
                r.LatestHistory = _historyRepo.GetLatestHistoryOfRoom(r.Id);
            });

            return listRoom.OrderByDescending(room => room.LatestHistory.EndDate).ToList();
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
