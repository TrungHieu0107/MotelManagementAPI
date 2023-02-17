using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class History
    {
        [Key]
        public long Id { get; set; }
        public long RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room Room { get; set; }
        public long ResidentId { get; set; }
        [ForeignKey("ResidentId")]
        public Resident Resident { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
