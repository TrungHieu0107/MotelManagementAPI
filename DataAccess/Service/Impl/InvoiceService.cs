using BussinessObject.Models;
using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using BussinessObject.Status;

namespace DataAccess.Service.Impl
{
    public class InvoiceService : IInvoiceService
    {
        public Invoice AddInititalInvoice(long residentId, long roomId, DateTime startDate)
        {
            throw new NotImplementedException();
        }
        private readonly IInvoiceRepo _invoiceRepo;

        public InvoiceService(IInvoiceRepo invoiceRepo)
        {
            _invoiceRepo = invoiceRepo;
        }
        public List<InvoiceDTO> GetInvoiceHistoryOfRoom(long roomId, ref Pagination pagination)
        {
            pagination.Total = _invoiceRepo.CountInvocieHistoryHasRoomId(roomId);
            return _invoiceRepo.GetInvoiceHistoryOfRoomWithPaging(roomId, pagination).ToList();
        }

      
        public InvoiceDTO checkUnPayInvocieByRoomCode(String roomCode)
        {
            return _invoiceRepo.GetInvoiceHistoryOfRoomWithUnPayStatus(roomCode).FirstOrDefault();
        }

        public int PayInvoice(String roomCode)
        {
            InvoiceDTO UnPaidInvoice = _invoiceRepo.GetInvoiceHistoryOfRoomWithUnPayStatus(roomCode).FirstOrDefault();

            if(UnPaidInvoice != null)
            {

                var invocie = _invoiceRepo.findById(UnPaidInvoice.Id);
                invocie.PaidDate = DateTime.Now;
                invocie.Status = InvoiceStatus.PAID;
                return _invoiceRepo.updateInvoiceStatus(invocie);

            } else
            {
                return 0;
            }
        }
    }
}
