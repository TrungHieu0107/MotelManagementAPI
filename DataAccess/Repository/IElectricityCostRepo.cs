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
    }
}
