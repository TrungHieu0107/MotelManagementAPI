using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using BussinessObject.CommonConstant;
using DataAccess.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MotelManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoomController(IRoomService roomService, IHttpContextAccessor httpContextAccessor)
        {
            this._roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
            _httpContextAccessor = httpContextAccessor;
        }


        [Authorize(Roles = "Manager")]
        [HttpPost]
        [Route("add-new-room")]
        public async Task<IActionResult> AddNewRoom(string Code, DateTime FeeAppliedDate, long RentFee, int Status)
        {
            
            CommonResponse commonResponse = new CommonResponse();

            try
            {
                var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.Claims.FirstOrDefault(a => a.Type == "Id")?.Value;

                var result = _roomService.AddNewRoom(Code, RentFee, FeeAppliedDate, Status, long.Parse(userId));
                if (result == null)
                {
                    commonResponse.Message = "Something Went Wrong";
                    return StatusCode(StatusCodes.Status500InternalServerError, commonResponse);
                }
                commonResponse.Data = result;
                return Ok(commonResponse);
            }
            catch (Exception ex)
            {
                commonResponse.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, commonResponse);
            }
        }


        [HttpPost]
        [Route("update-room")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateRoom(RoomDTO room)
        {
            CommonResponse commonResponse = new CommonResponse();
            try
            {
                var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.Claims.FirstOrDefault(a => a.Type == "Id")?.Value;

                var result = _roomService.UpdateRoom(room, long.Parse(userId));
                commonResponse.Data = result;
                return Ok(commonResponse);
            }
            catch (Exception ex)
            {
                commonResponse.Message=ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, commonResponse);
            }
        }


        [HttpGet]
        [Route("delete-room")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteRoom(long id)
        {
            CommonResponse commonResponse = new CommonResponse();
            try
            {
                var result = _roomService.DeleteRoomById(id);
                if (result)
                {
                    return Ok(result);
                } else
                {
                    commonResponse.Message = "Something Went Wrong";
                    return StatusCode(StatusCodes.Status400BadRequest,commonResponse);
                }
            }
            catch(TaskCanceledException e)
            {
                commonResponse.Message = e.Message;
                return StatusCode(StatusCodes.Status400BadRequest, commonResponse);
            }
            catch (Exception ex)
            {
                commonResponse.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, commonResponse);
            }
        }

        [HttpGet]
        [Route("get-rooms")]
        [Authorize(Roles = Role.MANAGER)]
        public async Task<IActionResult> GetAllRoomHistoryWithFilter
        (
            string? roomCode,
            long? minFee,
            long? maxFee,
            int? status,
            DateTime? appliedDateAfter,
            int? currentPage,
            int? pageSize
        )
        {
            CommonResponse commonResponse = new CommonResponse();
            try
            {
                var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.Claims.FirstOrDefault(a => a.Type == "Id")?.Value; 

                Pagination pagination = new Pagination();
                pagination.PageSize = pageSize ?? 10;
                pagination.CurrentPage = currentPage ?? 1;
                var result = _roomService.GetAllRoomHistoryWithFilter
                                (
                                     roomCode ?? null,
                                     minFee ?? -1,
                                     maxFee ?? -1,
                                     status ?? -1,
                                     appliedDateAfter ?? DateTime.MinValue,
                                     ref pagination, 
                                     long.Parse(userId)
                                );

                commonResponse.Data = result;
                commonResponse.Pagination = pagination;
                return Ok(commonResponse);
            }
            catch (Exception ex)
            {
                commonResponse.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, commonResponse);
            }
        }

        [HttpGet]
        [Route("detail")]
        [Authorize(Roles = "Manager")]
        public IActionResult GetRoomDTOForDetailById(long roomId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                long managerId = long.Parse(claimsIdentity.Claims.FirstOrDefault(a => a.Type == "Id")?.Value);

                RoomDTOForDetail roomDTOForDetail = _roomService.FindByIdForManager(roomId, managerId);
                response.Data = roomDTOForDetail;
                response.Message = "Get room successfully";
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
        }

        [HttpGet]
        [Route("detail-for-resident")]
        [Authorize(Roles = "Resident")]
        public IActionResult GetRoomDTOForDetailForResidentById(long roomId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                long residentId = long.Parse(claimsIdentity.Claims.FirstOrDefault(a => a.Type == "Id")?.Value);

                RoomDTOForDetail roomDTOForDetail = _roomService.FindByIdForResident(roomId, residentId);
                response.Data = roomDTOForDetail;
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
