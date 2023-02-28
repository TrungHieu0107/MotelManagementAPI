using BussinessObject.Status;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObject.Models
{
    public class MotelChain
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public MotelChainStatus Status { get; set; }
        public long? ManagerId { get; set; }

        [ForeignKey("ManagerId")]
        public Manager Manager { get; set; }
        public virtual List<Room> Rooms { get; set; }
    }
}
