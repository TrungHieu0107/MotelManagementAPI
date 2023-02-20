using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.DTO
{
    public class ElectricityCostRequestDTO
    {
     
        public long Price { get; set; }
        public int AppliedMonth { get; set; }
        public int AppliedYear { get; set;}
    }
}
