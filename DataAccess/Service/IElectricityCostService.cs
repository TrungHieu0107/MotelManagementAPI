using BussinessObject.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public interface IElectricityCostService
    {
        Task<IEnumerable<ElectricityCostDTO>> GetElectrictyCost(int year, int month, int curentPage, int pageSize);
        Task<ElectricityCostDTO> GetCurrentElectricityCost();

        ElectricityCostDTO UpdateElectricity(ElectricityCostRequestDTO obj);
    }
}
