using BussinessObject.DTO.Common;
using BussinessObject.DTO;
using DataAccess.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace MotelManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaterCostController : ControllerBase
    {

        private readonly IWaterCostService _waterCostService;
        public WaterCostController(IWaterCostService waterCostService)
        {
            this._waterCostService = waterCostService ??
                throw new ArgumentNullException(nameof(waterCostService));
        }

        /// <summary>
        /// Xem giá nuoc thay đổi dựa theo năm, truyền vào giá trị -1 thì sẽ trả về tất cả các giá nuoc
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("water-cost/{year}/{month}")]
        public async Task<IActionResult> Get(int year, int month)
        {

            var waterCost = await _waterCostService.GetWaterCost(month, year);
            CommonResponse common = new CommonResponse();
            if (waterCost == null)
            {
                common.Message = "Some thing went wrong";
            }
            else
            {
                common.Data = waterCost;

            }
            return Ok(common);
        }


        /// <summary>
        /// lấy giá tiền của tiền nuoc hiện tại
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-current-water-cost")]
        public async Task<IActionResult> GetCurentWaterCost()
        {
            var waterCost = await _waterCostService.GetCurrentWaterCost();
            CommonResponse common = new CommonResponse();
            if (waterCost == null)
            {
                common.Message = "Not Found";
            }
            else
            {
                common.Data = waterCost;

            }
            return Ok(common);
        }

        [HttpPost]
        [Route("add-water-cost")]
        public async Task<IActionResult> Post(WaterRequestDTO obj)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _waterCostService.UpdateWaterCost(obj);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Something Went Wrong");

            return Ok("Added Successfully");
        }
    }
}
