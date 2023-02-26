using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using DataAccess.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MotelManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResidentController : ControllerBase
    {
        private readonly IResidentService _residentService;

        public ResidentController(IResidentService residentService)
        {
            this._residentService = residentService;
        }

     
        // lấy toàn bộ tài khoản có cmnd tương ứng
        [HttpGet]
        [Route("resident/{idCard}")]
        public IActionResult GetResidentAccountByIdentityCard(string idCard)
        {
            CommonResponse common = new CommonResponse();
            try
            {
                var rs = _residentService.GetResidentByIdentityCardNumber(idCard);
                
                
                if (rs == null)
                {
                    common.Message = "Not found";
                }
                else
                {
                    common.Data = rs;

                }
                return Ok(common);
            } catch(Exception ex)
            {
                common.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, common);

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
            } catch(Exception ex)
            {
                CommonResponse common = new CommonResponse();
                common.Message = ex.Message;
                return StatusCode(StatusCodes.Status400BadRequest, common);
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


        [HttpPost]
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
