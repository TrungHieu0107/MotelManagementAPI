using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using DataAccess.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MotelManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        /// 
        [Authorize(Roles = "Manager")]
        [HttpGet]
        [Route("water-cost/{year}/{month}/{pageSize}/{currentPage}")]
        public async Task<IActionResult> Get(int year, int month, int currentPage, int pageSize)
        {
            CommonResponse common = new CommonResponse();
            try
            {
                Pagination pagination = new Pagination();
                pagination.PageSize = pageSize;
                pagination.CurrentPage = currentPage;

                var waterCost = await _waterCostService.GetWaterCost(month, year, currentPage, pageSize);

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
            catch (Exception ex)
            {
                common.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, common);
            }


        }


        /// <summary>
        /// lấy giá tiền của tiền nuoc hiện tại
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Manager")]
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
        [Authorize(Roles = "Manager")]
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
