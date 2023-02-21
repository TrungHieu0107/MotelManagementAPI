using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IWaterCostRepo
    {
        Task<WaterCost> GetCurrentWaterCost();

        Task<IEnumerable<WaterCost>> GetWaterCostByMonthAndYear(int month, int year, int currentPage, int pageSize);
        WaterCost GetWaterCostAfterDate(DateTime date);

        int AddWaterCost(WaterCost WaterCost);

        int UpdateWaterCost(WaterCost WaterCost);
        Task<WaterCost> GetWaterCostOfInvoiceById(long id);
    }
}
