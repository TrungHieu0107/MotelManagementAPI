using BussinessObject.DTO;
using BussinessObject.Models;

namespace DataAccess.Repository
{
    public interface IHistoryRepo
    {
        History checkResidentBookingHistoryByResidentId(long residentId);
        HistoryDTO Update(HistoryDTO history);
        HistoryDTO GetLatestHistoryByRoomId(long id);
    }
}
