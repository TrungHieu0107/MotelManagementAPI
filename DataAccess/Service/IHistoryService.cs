using BussinessObject.Models;
using System;

namespace DataAccess.Service
{
    public interface IHistoryService
    {
        public History AddNewHistory(long residentId, long roomId, DateTime startDate);
    }
}
