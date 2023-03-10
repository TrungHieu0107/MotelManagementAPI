using BussinessObject.CommonConstant;
using BussinessObject.DTO;
using BussinessObject.Models;
using DataAccess.Repository;
using System;
using System.Collections.Generic;

namespace DataAccess.Service.Impl
{
    public class HistoryService : IHistoryService
    {
        private readonly IHistoryRepo _historyRepo;
        private readonly IInvoiceRepo _invoiceRepo;
        private readonly IRoomRepo _roomRepo;

        public HistoryService(IHistoryRepo historyRepo, IInvoiceRepo invoiceRepo, IRoomRepo roomRepo)
        {
            _historyRepo = historyRepo;
            _invoiceRepo = invoiceRepo;
            _roomRepo = roomRepo;
        }

        public History AddNewHistory(long residentId, long roomId, DateTime startDate)
        {
            History history = new History();
            history.RoomId = roomId;
            history.StartDate = startDate;
            history.ResidentId = residentId;
            history.EndDate = null;
            return _historyRepo.Add(history);
        }

        public List<InvoiceDTO> CheckInvoiceCheckOutDateForResident(long residentId, long managerId, long roomId, DateTime checkoutDate)
        {
            _roomRepo.FindByIdForManager(roomId, managerId);
            List<InvoiceDTO> invoiceDTOs = _invoiceRepo.UpdateCheckOutDateForResident(residentId, roomId, checkoutDate);
            return invoiceDTOs;
        }

        public List<InvoiceDTO> CheckInvoiceToCheckOutDateForResident(long residentId, long managerId, long roomId, DateTime checkoutDate)
        {
            List<InvoiceDTO> invoiceDTOs = _invoiceRepo.GetClosedSampleInvoicesThatNotPayYetByResidentIdAndRoomId(residentId, roomId, checkoutDate);
            return invoiceDTOs;
        }

        public List<InvoiceDTO> UpdateCheckOutDateForResident(long residentId, long managerId, long roomId, DateTime checkoutDate)
        {
            HistoryDTO result = _historyRepo.UpdateCheckoutDateForResident(residentId, managerId, roomId, checkoutDate);
            if (result != null)
            {
                List<InvoiceDTO> invoiceDTOs = _invoiceRepo.UpdateCheckOutDateForResident(residentId, roomId, checkoutDate);
                _roomRepo.UpdateCheckOutDateForResident(roomId);
                return invoiceDTOs;
            }
            else throw new Exception("Đã có lỗi xảy ra ở phía hệ thống.");
        }
    }
}
