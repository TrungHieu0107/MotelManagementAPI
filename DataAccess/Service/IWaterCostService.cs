using BussinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public interface IWaterCostService
    {
        Task<IEnumerable<WaterCostDTO>> GetWaterCost(int year, int month);
        Task<WaterCostDTO> GetCurrentWaterCost();

        WaterCostDTO UpdateWaterCost(WaterRequestDTO obj);
    }
}
