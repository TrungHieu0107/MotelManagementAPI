using BussinessObject.Models;
using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}
