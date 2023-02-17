using BussinessObject.Status;
using System;
using System.Collections.Generic;

namespace BussinessObject.DTO
{
    public class RoomDTO
    {
        long Id { get; set; }
        string Code { get; set; }
        long RentFee { get; set; }
        DateTime FeeAppliedDate { get; set; }
        RoomStatus Status { get; set; }
        long MotelId { get; set; }
        MotelChainDTO MotelChain { get; set; }
        List<HistoryDTO> Histories { get; set; }
        List<InvoiceDTO> Invoices { get; set; }
    }
}
