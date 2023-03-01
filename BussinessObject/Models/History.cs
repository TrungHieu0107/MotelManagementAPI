using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace BussinessObject.Models
{
    public class History
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public long RoomId { get; set; }
        [ForeignKey("RoomId")]
     
        public Room Room { get; set; }
        [Required]
        public long ResidentId { get; set; }
        [ForeignKey("ResidentId")]
      
        public Resident Resident { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
