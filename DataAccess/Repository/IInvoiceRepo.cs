using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using BussinessObject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IInvoiceRepo
    {
        List<Invoice> checkLateInvoice(string idCard);
        IEnumerable<InvoiceDTO> GetInvoiceHistoryOfRoomWithPaging(long roomId, Pagination pagination);

        long CountInvocieHistoryHasRoomId(long roomId);
    }
}
