using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IWaterCostRepo
    {
        Task<WaterCost> GetCurrentWaterCost();

        Task<IEnumerable<WaterCost>> GetWaterCostByMonthAndYear(int month, int year);
        WaterCost GetWaterCostAfterDate(DateTime date);

        void AddWaterCost(WaterCost WaterCost);

        void UpdateWaterCost(WaterCost WaterCost);
    }
}
