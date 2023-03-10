using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using BussinessObject.Models;
using BussinessObject.Status;
using DataAccess.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Security.Claims;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Threading.Tasks;

namespace MotelManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResidentController : ControllerBase
    {
        private readonly IResidentService _residentService;
        private readonly IHistoryService _historyService;
        private readonly IRoomService _roomService;
        private readonly IInvoiceService _invoiceService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResidentController(IResidentService residentService, IHistoryService historyService, IRoomService roomService, IInvoiceService invoiceService, IHttpContextAccessor httpContextAccessor)
        {
            _residentService = residentService;
            _historyService = historyService;
            _roomService = roomService;
            _invoiceService = invoiceService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Resident")]
        [Route("detail")]
        public IActionResult FindByIdForDetail(long? residentId, int pageSize, int currentPage, string roomStatus)
        {
            CommonResponse response = new CommonResponse();
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            long accountId = long.Parse(claimsIdentity.Claims.FirstOrDefault(a => a.Type == "Id")?.Value);
            string roleClaim = claimsIdentity.Claims.FirstOrDefault(a => a.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            try
            {
                try
                {
                    if (pageSize < 1 || currentPage < 1) throw new Exception("Page size and current page must be >= 1");
                    if(roleClaim == "Manager")
                    {
                        if (residentId == null) throw new Exception("Resident ID must not empty.");
                        return Ok(_residentService.FindByIdForDetail(residentId.Value, pageSize, currentPage, roomStatus));
                    }
                    else
                        return Ok(_residentService.FindByIdForDetail(accountId, pageSize, currentPage, roomStatus));
                }
                catch (Exception ex)
                {
                    response.Message = ex.Message;
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }
                
            } catch(Exception ex)
            {
                response.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        [Authorize(Roles = "Manager")]
        [HttpPost]
        [Route("add-resident")]
        public IActionResult AddResidentAccount(AccountDTO accountDTO)
        {
            try
            {
                var result = _residentService.CreatResidentAccount(accountDTO);

                if (!result)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Something Went Wrong");

                return Ok("Added Successfully");
            }catch (TaskCanceledException ex)
            {

                CommonResponse common = new CommonResponse();
                common.Message = ex.Message;
                return StatusCode(StatusCodes.Status400BadRequest, common);
            } 
            
            catch(Exception ex)
            {
                CommonResponse common = new CommonResponse();
                common.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, common);
            }
        }


        [Authorize(Roles = "Manager")]
        [HttpPut]
        [Route("deactive-resident")]
        public IActionResult DeActiveResident(string idCard)
        {
            try
            {
                bool result = _residentService.DeActiveResident(idCard);

                if (result)
                    return Ok("Success");

                else
                    return StatusCode(StatusCodes.Status400BadRequest, "Something went wrong please check data again");
            } catch(Exception ex)
            {
                CommonResponse common = new CommonResponse();
                common.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, common);
            }
        }




        [Authorize(Roles = "Manager")]
        [HttpPut]
        [Route("active-resident")]
        public IActionResult ActiveResident(string idCard)
        {
            bool result = _residentService.ActiveResident(idCard);

            if (result)
                return Ok("Success");

            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong please check data again");
        }



        [Authorize(Roles = "Manager")]
        [HttpPut]
        [Route("update-resident")]
        public IActionResult UpdateResident(long id, ResidentUpdateDTO accountDTO)
        {
            try
            {

                

                var result = _residentService.UpdateResidentAccount(id,accountDTO);

                if (!result)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Update failed, please check again");

                return Ok("Added Successfully");
            }
            catch (TaskCanceledException ex)
            {
                CommonResponse common = new CommonResponse();
                common.Message = ex.Message;
                return StatusCode(StatusCodes.Status400BadRequest, common);
            }
            catch (Exception ex)
            {
                CommonResponse common = new CommonResponse();
                common.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, common);
            }
        }

        //[HttpGet]
        //[Route("find-by-identity-card-number-to-book-room")]
        //public IActionResult FindByIdentityCardNumberToBookRoom(string identityCardNumber)
        //{
        //    try
        //    {
                
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonResponse common = new CommonResponse();
        //        common.Message = ex.Message;
        //        return StatusCode(StatusCodes.Status400BadRequest, common);
        //    }
        //}

        [Authorize(Roles = "Manager")]
        [HttpPost]
        [Route("book-room")]
        public IActionResult BookRoom(BookingRoomRequest bookingRoomRequest)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                long managerId = long.Parse(claimsIdentity.Claims.FirstOrDefault(a => a.Type == "Id")?.Value);
                DateTime startDate = bookingRoomRequest.StartDate;

                Resident resident = new Resident();
                Room room = new Room();
                try
                {
                    _residentService.BookRoom(bookingRoomRequest, managerId);
                    _invoiceService.AddInitialInvoice(resident.Id, bookingRoomRequest.RoomId, startDate);
                }
                catch (Exception ex)
                {
                    response.Message = ex.Message;
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }

                response.Data = bookingRoomRequest.RoomId;
                response.Message = "Booked room successfully.";

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [Route("get-all-resident")]
        public IActionResult getAllResident(int pageSize = 10, int currentPage = 1)
        {
            CommonResponse commonResponse = new CommonResponse();
            try
            {
                var list = _residentService.getAllResident(pageSize,currentPage);
                commonResponse.Data = list;
                return Ok(commonResponse);
            }
            catch (Exception ex)
            {
                commonResponse.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, commonResponse);


            }

        }


        [HttpGet]
        [Route("filter-resident")]
        public IActionResult FillterResident(string idCardNumber,string phone, string Fullname,int status,int pageSize, int currentPage)
        {
            CommonResponse commonResponse = new CommonResponse();
            try
            {
                var list = _residentService.FillterResident(idCardNumber, phone, Fullname,status,  pageSize, currentPage);
                commonResponse.Data = list;
                return Ok(commonResponse);
            }
            catch (Exception ex)
            {
                commonResponse.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, commonResponse);


            }

        }

        [Authorize(Roles = "Resident")]
        [HttpPut]
        [Route("update-information")]
        public IActionResult UpdateInformationForResident( ResidentUpdateDTO accountDTO)
        {
            try
            {
                // Ensure that the authenticated user is the same as the user being updated
                var userId = int.Parse(User.FindFirst("Id").Value);
                
                var result = _residentService.UpdateResidentAccount(userId, accountDTO);

                if (!result)
                    return StatusCode(StatusCodes.Status400BadRequest, "Identity cart number already exist");

                return Ok("Added Successfully");
            }
            catch (Exception ex)
            {
                CommonResponse common = new CommonResponse();
                common.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, common);

            }


        }


                
    }
}
