using BussinessObject.Status;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObject.Models
{
    public class Room
    {
        [Key]
        public long Id { get; set; }
        public string Code { get; set; }
        public long RentFee { get; set; }
        public DateTime? FeeAppliedDate { get; set; }
        public RoomStatus Status { get; set; }
        public long MotelId { get; set; }
        [ForeignKey("MotelId")]
        public MotelChain MotelChain { get; set; }
        public virtual List<History> Histories { get; set; }
        public virtual List<Invoice> Invoices { get; set; }
    }
}