using BussinessObject.Data;
using BussinessObject.DTO;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;

namespace DataAccess.Repository
{
    public class MotelChainRepo : IMotelChainRepo
    {

        private readonly Context _context;

        public MotelChainRepo(Context context)
        {
            _context = context;
        }
        public MotelChainDTO GetMotelWithManagerId(long managerId)
        {

            MotelChainDTO motelChainDTO = _context.MotelChains.Where(x => x.ManagerId == managerId).Select(motel => new MotelChainDTO()
            {
                ManagerId = motel.ManagerId.Value,
                Id = motel.Id,
                Name = motel.Name,
                Status = motel.Status,
                Address = motel.Address,
            }).FirstOrDefault();

            return motelChainDTO;
        }
    }
}
