using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using BussinessObject.CommonConstant;
using DataAccess.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MotelManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;

        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpGet]
        [Route("update-check-out-date")]
        [Authorize(Roles = Role.MANAGER)]
        public IActionResult UpdateCheckOutDate(long residentId, long roomId, DateTime checkoutDate)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                HistoryDTO historyDTO = _historyService.UpdateCheckOutDateForResident(residentId, roomId, checkoutDate);

                if(historyDTO == null)
                {
                    response.Message = "Cannot update check-out date for resident";
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }
                response.Data = historyDTO;
                response.Message = "Get room successfully";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
