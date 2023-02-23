using BussinessObject.Data;
using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class MotelChainRepo : IMotelChainRepo
    {
        private readonly Context _context;

        public MotelChainRepo(Context context)
        {
            _context = context;
        }

        public MotelChain FindById(long id)
        {
            MotelChain motelChain = _context.MotelChains.FirstOrDefault(x => x.Id == id);
            if (motelChain == null) throw new Exception("Motel with ID: " + id + " doesn't exist");
            return motelChain;
        }
    }
}
