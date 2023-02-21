using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using DataAccess.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotelManagementAPI.Controllers
{
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

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }
        [HttpGet]
        [Route("add-new-room")]
        public async Task<IActionResult> AddNewRoom(string code, long rentFee, string feeAppliedDate, int status, long motelID)
        {
            CommonResponse commonResponse = new CommonResponse();

            try
            {
                
                var result = _roomService.AddNewRoom(code, rentFee, feeAppliedDate, status, motelID);
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
                var result = _roomService.UpdateRoom(room);
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
        [Route("get-rooms-filter")]
        public async Task<IActionResult> GetAllRoomHistoryWithFilter
        (
            long motelId,
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
                Pagination pagination = new Pagination();
                pagination.PageSize = pageSize ?? 10;
                pagination.CurrentPage = currentPage ?? 1;
                var result = _roomService.GetAllRoomHistoryWithFilter
                                (
                                     motelId,
                                     roomCode,
                                     minFee,
                                     maxFee,
                                     status,
                                     appliedDateAfter,
                                     ref pagination
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
