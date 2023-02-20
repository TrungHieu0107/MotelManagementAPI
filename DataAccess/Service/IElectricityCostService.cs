using BussinessObject.DTO;
using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public interface IElectricityCostService
    {
        Task<IEnumerable<ElectricityCostDTO>> GetElectrictyCost(int year, int month);
        Task<ElectricityCostDTO> GetCurrentElectricityCost();

        ElectricityCostDTO UpdateElectricity(ElectricityCostRequestDTO obj);
    }
}
