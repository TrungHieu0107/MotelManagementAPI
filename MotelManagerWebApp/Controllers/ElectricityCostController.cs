using Microsoft.AspNetCore.Mvc;
using MotelManagerWebApp.Service;
using System.Threading.Tasks;

namespace MotelManagerWebApp.Controllers
{
    public class ElectricityCostController : Controller
    {
        private readonly IElectricityCostApiClientcs _electricityCostApiClientcs;

        public ElectricityCostController(IElectricityCostApiClientcs electricityCostApiClientcs) { 
            this._electricityCostApiClientcs = electricityCostApiClientcs;
        }
        [Route("ElectricityCost")]
        public async Task<IActionResult> ElectricityCost()
        {
            await _electricityCostApiClientcs.getElectricityCost();
            return View();
        }
    }
}
