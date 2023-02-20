using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.DTO;
using BussinessObject.Models;

namespace DataAccess.Repository
{
    public interface IElectricityRepo
    {
        Task<ElectricityCost> GetCurrentElectrictyCost();

        Task<IEnumerable<ElectricityCost>> GetElectricityCostByMonthAndYear(int month, int year);
        ElectricityCost GetElectricitAfterDate(DateTime date);

        void AddElectricityCost(ElectricityCost electricityCost);

        void UpdateElectricityCost(ElectricityCost electricityCost);
    }
}
