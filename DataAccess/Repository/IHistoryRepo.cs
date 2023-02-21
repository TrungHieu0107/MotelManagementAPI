using BussinessObject.Models;

namespace DataAccess.Repository
{
    public interface IHistoryRepo
    {
        History checkResidentBookingHistoryByResidentId(long residentId);
    }
}
