using BussinessObject.DTO;
using System.Threading.Tasks;

namespace MotelManagerWebApp.Service
{
    public interface IAccountApiClient
    {
        Task<string> Authenticate(LoginDTO loginDTO);
    }
}
