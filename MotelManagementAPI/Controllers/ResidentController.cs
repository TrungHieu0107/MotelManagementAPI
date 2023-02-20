using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using DataAccess.Repository;
using DataAccess.Service;
using DataAccess.Service.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
            var rs = _residentService.GetResidentByIdentityCardNumber(idCard);
            CommonResponse common = new CommonResponse();
            if (rs == null)
            {
                common.Message = "Not found";
            }
            else
            {
                common.Data = rs;

            }
            return Ok(common);
        }

        [HttpPost]
        [Route("add-resident")]
        public IActionResult AddResidentAccount(AccountDTO accountDTO)
        {
            var result = _residentService.CreatResidentAccount(accountDTO);

            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError, "Something Went Wrong");

            return Ok("Added Successfully");
        }

        [HttpPut]
        [Route("deactive-resident")]
        public IActionResult DeActiveResident(string idCard)
        {
            bool result = _residentService.DeActiveResident(idCard);

            if (result)
                return Ok("Success");

            else
                return StatusCode(StatusCodes.Status400BadRequest, "Count not find the resident");
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
            var result = _residentService.UpdateResidentAccount(id, accountDTO);
          
            CommonResponse common = new CommonResponse();
            if (!result)
            {
                common.Message = "Some thing went wrong";
            }
            else
            {
                common.Data = result;

            }
            return Ok(common);

           
        }


    }
}
