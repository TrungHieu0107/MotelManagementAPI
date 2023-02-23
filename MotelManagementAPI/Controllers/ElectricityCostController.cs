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
        [Route("electricity-cost/{year}/{month}/{pageSize}/{currentPage}")]
        public async Task<IActionResult> Get(int year, int month, int pageSize = 10, int currentPage = 1)
        {

            CommonResponse common = new CommonResponse();
            try
            {
                Pagination pagination = new Pagination();


                pagination.PageSize = pageSize;
                pagination.CurrentPage = currentPage;
                var electricityCost = await electricityCostService.GetElectrictyCost(month, year, currentPage, pageSize);

                if (electricityCost == null)
                {
                    common.Message = "Some thing went wrong";
                }
                else
                {
                    common.Data = electricityCost;

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
        /// lấy giá tiền của tiền điện hiện tại
        /// </summary>
        /// <returns></returns>
        // [Authorize(Roles = "Manager")]
        // [Authorize(Roles = "Resident")]

        [HttpGet]
        [Route("get-current-electricity-cost")]
        public async Task<IActionResult> GetCurentElectricityCost()
        {
            CommonResponse common = new CommonResponse();
            try
            {
                var electricityCost = await electricityCostService.GetCurrentElectricityCost();

                if (electricityCost == null)
                {
                    common.Message = "Not Found";
                }
                else
                {
                    common.Data = electricityCost;

                }
                return Ok(common);
            }
            catch (Exception ex)
            {
                common.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, common);

            }
        }

        [HttpPost]
        [Route("add-electricity-cost")]
        public async Task<IActionResult> Post(ElectricityCostRequestDTO obj)
        {
            CommonResponse common = new CommonResponse();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = electricityCostService.UpdateElectricity(obj);
                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Something Went Wrong");

                return Ok("Added Successfully");
            }
            catch (Exception ex)
            {
                common.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, common);

            }
        }
    }
}
