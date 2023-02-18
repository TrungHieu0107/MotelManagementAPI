using BussinessObject.Status;
using System;
using System.Collections.Generic;

namespace BussinessObject.DTO
{
    public class RoomDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public long RentFee { get; set; }
        public DateTime FeeAppliedDate { get; set; }
        public RoomStatus Status { get; set; }
        public long MotelId { get; set; }
        public MotelChainDTO MotelChain { get; set; }
        public List<HistoryDTO> Histories { get; set; }
        public List<InvoiceDTO> Invoices { get; set; }
    }
}
