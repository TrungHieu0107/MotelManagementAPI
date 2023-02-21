using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using DataAccess.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MotelManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                    return StatusCode(StatusCodes.Status400BadRequest, "Count not find the resident");
            } catch(Exception ex)
            {
                CommonResponse common = new CommonResponse();
                common.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, common);
            }
        }

        [HttpPut]
        [Route("active-resident")]
        public IActionResult ActiveResident(string idCard)
        {
            bool result = _residentService.ActiveResident(idCard);

            if (result)
                return Ok("Success");

            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Count not find the resident");
        }

        [HttpPut]
        [Route("update-resident")]
        public IActionResult UpdateResident(long id, ResidentUpdateDTO accountDTO)
        {
            CommonResponse common = new CommonResponse();
            try
            {
                var result = _residentService.UpdateResidentAccount(id, accountDTO);
              
                if (!result)
                {
                    common.Message = "Some thing went wrong";
                }
                else
                {
                    common.Data = result;

                }
                return Ok(common);
            } catch (Exception ex)
            {
                common.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, common);
            }


        }


    }
}
