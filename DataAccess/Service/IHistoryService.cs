using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public interface IHistoryService
    {
        public History AddNewHistory(long residentId, long roomId, DateTime startDate);
    }
}
