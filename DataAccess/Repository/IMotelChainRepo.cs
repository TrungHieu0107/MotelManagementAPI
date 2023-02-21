using BussinessObject.DTO;

namespace DataAccess.Repository
{
    public interface IMotelChainRepo
    {
        MotelChainDTO GetMotelWithManagerId(long managerId);
    }
}
