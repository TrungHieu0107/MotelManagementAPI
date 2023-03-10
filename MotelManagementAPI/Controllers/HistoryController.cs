using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using BussinessObject.CommonConstant;
using DataAccess.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;

namespace MotelManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HistoryController(IHistoryService historyService, IHttpContextAccessor httpContextAccessor)
        {
            _historyService = historyService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("check-invoice-to-check-out-date")]
        [Authorize(Roles = Role.MANAGER)]
        public IActionResult CheckInvoiceToCheckOutDate(long residentId, long roomId, DateTime checkoutDate)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                long managerId = long.Parse(claimsIdentity.Claims.FirstOrDefault(a => a.Type == "Id")?.Value);
                List<InvoiceDTO> invoiceDTOs = _historyService.CheckInvoiceToCheckOutDateForResident(residentId, managerId, roomId, checkoutDate);
                response.Data = invoiceDTOs;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [Route("update-check-out-date")]
        [Authorize(Roles = Role.MANAGER)]
        public IActionResult UpdateCheckOutDate(long residentId, long roomId, DateTime checkoutDate)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                long managerId = long.Parse(claimsIdentity.Claims.FirstOrDefault(a => a.Type == "Id")?.Value);
                List<InvoiceDTO> invoiceDTOs = _historyService.UpdateCheckOutDateForResident(residentId, managerId, roomId, checkoutDate);
                response.Data = invoiceDTOs;
                response.Message = "Kết thúc thuê phòng thành công.";
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
