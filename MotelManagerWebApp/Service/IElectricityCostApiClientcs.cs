using BussinessObject.DTO.Common;
using System.Threading.Tasks;

namespace MotelManagerWebApp.Service
{
    public interface IElectricityCostApiClientcs
    {
        Task<CommonResponse> getElectricityCost();
    }
}
