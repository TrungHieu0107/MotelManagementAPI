using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.DTO.Common
{
    public class CommonResponse
    {
        public object Data { get; set; }
        public Pagination Pagination { get; set; }
        public string Message { get; set; }

    }
}
