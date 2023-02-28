using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObject.Models
{
    public class ElectricityCost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public long Price { get; set; }
        [Required]
        public DateTime AppliedDate { get; set; }
        public virtual List<Invoice> Invoices { get; set; }
    }
}
