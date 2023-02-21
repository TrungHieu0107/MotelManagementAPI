using BussinessObject.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public interface IWaterCostService
    {
        Task<IEnumerable<WaterCostDTO>> GetWaterCost(int year, int month, int currentPage, int pageSize);
        Task<WaterCostDTO> GetCurrentWaterCost();

        WaterCostDTO UpdateWaterCost(WaterRequestDTO obj);
    }
}
