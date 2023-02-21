using BussinessObject.Data;
using BussinessObject.Models;
using BussinessObject.Status;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

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
    } 
}
