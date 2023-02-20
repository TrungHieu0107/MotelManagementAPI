using BussinessObject.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using BussinessObject.DTO.Common;
using DataAccess.Service;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;

namespace MotelManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ElectricityCostController : ControllerBase
    {
        private readonly IElectricityCostService electricityCostService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ElectricityCostController(IElectricityCostService electricityCostService, IHttpContextAccessor httpContextAccessor)
        {
            this.electricityCostService = electricityCostService ??
                throw new ArgumentNullException(nameof(electricityCostService));
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Xem giá điện thay đổi dựa theo năm, truyền vào giá trị -1 thì sẽ trả về tất cả các giá điện
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("electricity-cost/{year}/{month}")]
        public async Task<IActionResult> Get(int year, int month)
        {
           // var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
           // var userId = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            var electricityCost = await electricityCostService.GetElectrictyCost(month, year);
            CommonResponse common = new CommonResponse();
            if (electricityCost == null) {
                common.Message = "Some thing went wrong";
            } else
            {
                common.Data = electricityCost;

            }
            return Ok(common); 
        }


        /// <summary>
        /// lấy giá tiền của tiền điện hiện tại
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Manager")]
        [Authorize(Roles = "Resident")]

        [HttpGet]
        [Route("get-current-electricity-cost")]
        public async Task<IActionResult> GetCurentElectricityCost()
        {
            var electricityCost = await electricityCostService.GetCurrentElectricityCost();
            CommonResponse common = new CommonResponse();
            if (electricityCost == null)
            {
                common.Message = "Not Found";
            }
            else
            {
                common.Data = electricityCost;

            }
            return  Ok(common);
        }

        [HttpPost]
        [Route("add-electricity-cost")]
        public async Task<IActionResult> Post(ElectricityCostRequestDTO obj)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = electricityCostService.UpdateElectricity(obj);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Something Went Wrong");

            return Ok("Added Successfully");
        }
    }
}
