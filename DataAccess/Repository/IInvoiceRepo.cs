using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using BussinessObject.Models;
using BussinessObject.Status;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IInvoiceRepo
    {
        List<Invoice> checkLateInvoice(string idCard);
        IEnumerable<InvoiceDTO> GetInvoiceHistoryOfRoomWithPaging(long roomId, Pagination pagination);
        long CountInvocieHistoryHasRoomId(long roomId);
        IEnumerable<InvoiceDTO> GetInvoiceHistoryOfRoomNotPaidYet(string  RoomCode);
        int UpdateInvoiceStatus(Invoice invoice);
        InvoiceDTO GetInvoiceDetailById(long id);
        public Invoice GetPreviousInvoiceByRoomId(long roomId);
        public List<Invoice> AutoCheckLateInvoices(DateTime dateTime);
        public void Add(Invoice invoice);
        public Invoice FindById(long id);
        public bool AutoCloseInvoices(DateTime dateTime);
        IEnumerable<InvoiceDTO> GetAllInvoice(string roomCode, int status, long userId, ref Pagination pagination);
        InvoiceDTO UpdateRoomIdfOfInvoice(long newRoomId, long oldRoomId);
    }
}
