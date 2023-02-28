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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public string Code { get; set; }

        [Required]
        public long RentFee { get; set; }

        [Required]
        public DateTime FeeAppliedDate { get; set; }

        [Required]
        public RoomStatus Status { get; set; }

        [Required]
        public long MotelId { get; set; }
        [ForeignKey("MotelId")]

        [Required]
        public MotelChain MotelChain { get; set; }
        public virtual List<History> Histories { get; set; }
        public virtual List<Invoice> Invoices { get; set; }

    }
}