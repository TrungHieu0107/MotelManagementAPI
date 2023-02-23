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
        Task<IEnumerable<Invoice>> GetInvoiceOfRoom(long roomId, int? pageNumber, int? pageSize);
        IEnumerable<InvoiceDTO> GetInvoiceHistoryOfRoomWithPaging(long roomId, Pagination pagination);
        long CountInvocieHistoryHasRoomId(long roomId);
        IEnumerable<InvoiceDTO> GetInvoiceHistoryOfRoomWithUnPayStatus(string  RoomCode);
        int updateInvoiceStatus(Invoice invoice);
        Invoice findById(long id);
        public Invoice GetPreviousInvoiceByRoomId(long roomId);
        public List<Invoice> AutoCheckLateInvoices(DateTime dateTime);
        public void Add(Invoice invoice);
        public Invoice FindById(long id);
        public bool AutoCloseInvoices(DateTime dateTime);
    }
}
