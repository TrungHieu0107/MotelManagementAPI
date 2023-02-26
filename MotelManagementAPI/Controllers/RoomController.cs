using BussinessObject.DTO;
using BussinessObject.DTO.Common;
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
    [Authorize(Roles = "Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        //private readonly IRoomService _roomService;

        //public RoomController(IRoomService roomService) {
        //    this._roomService = roomService;
        //}

        //[HttpPut]
        //[Route("/booking-room")]
        //public IActionResult BookingRoom(string code, string idCard, DateTime bookedDate)
        //{
        //    CommonResponse common = new CommonResponse();
        //    try
        //    {
        //      var room = _roomService.bookRoom(code, idCard, bookedDate);
        //        if(room != null)
        //        {
        //            common.Message = "Booking room successfully";
        //            return Ok(common);
        //        } else
        //        {
        //            throw new Exception("Can not book room");
        //        }
        //    } catch (Exception ex)
        //    {
               
        //        common.Message = ex.Message;
        //        return StatusCode(StatusCodes.Status500InternalServerError, common);
        //    }
        //}


        //[HttpPut]
        //[Route("/booking-room")]
        //public IActionResult CancelBookingRoom(string code, string idCard, DateTime bookedDate)
        //{
        //    CommonResponse common = new CommonResponse();
        //    try
        //    {
        //        var room = _roomService.CancelBookRoom(code);
        //        if (room != null)
        //        {
        //            common.Message = "Booking room successfully";
        //            return Ok(common);
        //        }
        //        else
        //        {
        //            throw new Exception("Can not cancel book room");
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        common.Message = ex.Message;
        //        return StatusCode(StatusCodes.Status500InternalServerError, common);
        //    }
        //}


        private readonly IRoomService _roomService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoomController(IRoomService roomService, IHttpContextAccessor httpContextAccessor)
        {
            this._roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        [Route("add-new-room")]
        public async Task<IActionResult> AddNewRoom(string code, long rentFee, string feeAppliedDate, int status)
        {
            
            CommonResponse commonResponse = new CommonResponse();

            try
            {
                var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.Claims.FirstOrDefault(a => a.Type == "Id")?.Value;

                var result = _roomService.AddNewRoom(code, rentFee, feeAppliedDate, status, long.Parse(userId));
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
            catch (Exception ex)
            {
                commonResponse.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, commonResponse);
            }
        }

        [HttpPost]
        [Route("get-rooms")]
        public async Task<IActionResult> GetAllRoomHistoryWithFilter
        (
            string roomCode,
            long minFee,
            long maxFee,
            List<int> status,
            string appliedDateAfter,
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
                                     roomCode,
                                     minFee,
                                     maxFee,
                                     status,
                                     appliedDateAfter,
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
    }
}
