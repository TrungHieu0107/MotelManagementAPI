using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IElectricityRepo
    {
        Task<ElectricityCost> GetCurrentElectrictyCost();

        Task<IEnumerable<ElectricityCost>> GetElectricityCostByMonthAndYear(int month, int year, int currentPage, int pageSize);
        ElectricityCost GetElectricitAfterDate(DateTime date);

        int AddElectricityCost(ElectricityCost electricityCost);

        int UpdateElectricityCost(ElectricityCost electricityCost);
    }
}
