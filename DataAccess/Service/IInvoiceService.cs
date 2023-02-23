using BussinessObject.Models;
using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using BussinessObject.Status;
using System;
using System.Collections.Generic;

namespace DataAccess.Service
{
    public interface IInvoiceService
    {
        Invoice AddInititalInvoice(long residentId, long roomId, DateTime startDate);
        List<InvoiceDTO> GetInvoiceHistoryOfRoom(long roomId, ref Pagination pagination);
        InvoiceDTO checkUnPayInvocieByRoomCode(String roomCode);
        int PayInvoice(String roomCode);
        public bool AutoCreateInvoices(DateTime dateTime);
        public Invoice GetPreviousInvoiceByRoomId(long roomId);
        public bool AutoCheckLateInvoices(DateTime dateTime);
        public Invoice AddInitialInvoice(long residentId, long roomId, DateTime startDate);
        public Invoice FindById(long id);
        public bool AutoCloseInvoices(DateTime dateTime);
    }
}
