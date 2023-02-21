using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using DataAccess.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;

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


    } 
}
