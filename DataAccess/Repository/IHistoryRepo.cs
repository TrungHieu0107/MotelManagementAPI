using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IHistoryRepo
    {
        public List<History> GetNullEndDateHistories(DateTime dateTime);
        public History Add(History history);
        public List<History> GetHistoriesOfBookedUpToDateRooms(DateTime dateTime);
        public List<History> GetNullEndDateHistoriesByResident(Resident resident);
    }
}
