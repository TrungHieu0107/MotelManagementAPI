using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.Models;

namespace DataAccess.Repository
{
    public interface IElectricityCostRepo
    {
        public ElectricityCost GetElectricityCostForCreatingInvoicesByDate(DateTime dateTime);
        Task<ElectricityCost> GetCurrentElectrictyCost();
        Task<IEnumerable<ElectricityCost>> GetElectricityCostByMonthAndYear(int month, int year, int currentPage, int pageSize);
        ElectricityCost GetElectricitAfterDate(DateTime date);
        int AddElectricityCost(ElectricityCost electricityCost);
        int UpdateElectricityCost(ElectricityCost electricityCost);
        Task<ElectricityCost> GetElectricityCostById(long id);
    }
}
