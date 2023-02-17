using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class Manager : Account
    {
        public virtual MotelChain MotelChain { get; set; }
    }
}
