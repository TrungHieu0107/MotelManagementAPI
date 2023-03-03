using BussinessObject.Status;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BussinessObject.DTO
{
    public class RoomDTO
    {
        public long Id { get; set; }
        [Required]
        public string Code { get; set; }
        public long RentFee { get; set; }
        public DateTime FeeAppliedDate { get; set; }
        public RoomStatus Status { get; set; }
        public long MotelId { get; set; }
        public MotelChainDTO MotelChain { get; set; }
        public List<HistoryDTO> Histories { get; set; }
        public List<InvoiceDTO> Invoices { get; set; }
        public HistoryDTO LatestHistory { get; set; }
    }
}
