using BussinessObject.DTO;
using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IMotelChainRepo
    {
        MotelChainDTO GetMotelWithManagerId(long managerId);
        public MotelChain FindById(long id);
    }
}
