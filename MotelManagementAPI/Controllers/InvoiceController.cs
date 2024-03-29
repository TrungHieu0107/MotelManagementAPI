﻿using BussinessObject.DTO;
using BussinessObject.DTO.Common;
using BussinessObject.CommonConstant;
using DataAccess.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MotelManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InvoiceController(IInvoiceService invoiceService, IHttpContextAccessor httpContextAccessor)
        {
            _invoiceService = invoiceService;
            this._invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Roles = Role.MANAGER)]
        [HttpGet]
        [Route("get-invoice-history")]
        public async Task<IActionResult> GetInvoiceHistoryOfRoom(long roomId, int? currentPage, int? pageSize)
        {
            CommonResponse commonResponse = new CommonResponse();
            try
            {
                Pagination pagination = new Pagination();   
                pagination.CurrentPage = currentPage ?? 1;
                pagination.PageSize = pageSize ?? 10;

                var result = _invoiceService.GetInvoiceHistoryOfRoom(roomId, ref pagination);
                commonResponse.Pagination = pagination;
                commonResponse.Data = result;
                return Ok(commonResponse);
            }
            catch (Exception ex)
            {
                commonResponse.Message = ex.Message;    
                return StatusCode(StatusCodes.Status500InternalServerError, commonResponse);
            }
        }

        [Authorize(Roles = Role.MANAGER)]
        [HttpGet]
        [Route("get-unpay-invocie/{roomCode}")]
        public IActionResult GetUnpayInvocie(string roomCode)
        {
            CommonResponse commonResponse = new CommonResponse();
            try
            {
                var invocie = _invoiceService.CheckUnPayInvocieByRoomCode(roomCode);
              
                if (invocie == null)
                {
                    commonResponse.Message = "This room has been paid";
                }
                else
                {
                    commonResponse.Data = invocie;

                }
                return Ok(commonResponse);
            } catch (Exception ex)
            {
                commonResponse.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, commonResponse);
            }


        }

        [Authorize(Roles = Role.MANAGER)]
        [HttpGet]
        [Route("paid-invocie/{roomCode}")]
        public IActionResult PayInvocie(string roomCode)
        {
            CommonResponse commonResponse = new CommonResponse();
            try
            {
                int check = _invoiceService.PayInvoice(roomCode);

                if (check <= 0)
                {
                    commonResponse.Message = "Có lỗi xảy ra ở máy chủ";
                    return BadRequest(commonResponse);
                }
                else
                {
                    commonResponse.Message = "Thanh toán hóa đơn thành công";
                    return Ok(commonResponse);

                }
              
            }
            catch (Exception ex)
            {
                commonResponse.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, commonResponse);
            }


        }

        [Authorize(Roles = Role.RESIDENT_MANGER)]
        [HttpGet]
        [Route("get-invoice-detail")]
        public IActionResult GetInvoiceDetail(long id)
        {
            CommonResponse commonResponse = new CommonResponse();
            try
            {
                var userClaims = User as ClaimsPrincipal;
                var roleClaim = userClaims.FindFirstValue(ClaimTypes.Role);
                var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                long userId = long.Parse(claimsIdentity.Claims.FirstOrDefault(a => a.Type == "Id")?.Value);

                InvoiceDTO result = null;
                if (roleClaim == Role.MANAGER)
                {
                    result = _invoiceService.GetInvoiceDetailById(id, -1, userId);
                }
                else if (roleClaim == Role.RESIDENT)
                {
                    result = _invoiceService.GetInvoiceDetailById(id, userId, -1);
                }

                if(result == null)
                {
                    commonResponse.Message = "Not found this invoice with id " + id;
                    return BadRequest(commonResponse);
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

        [Authorize(Roles = Role.RESIDENT_MANGER)]
        [HttpGet]
        [Route("get-invoices")]
        public IActionResult GetInvoice(string? roomCode, int? status, DateTime? paidDate, long? userId, int? pageSize, int? currentPage)
        {
            CommonResponse commonResponse = new CommonResponse();
            Pagination pagination = new Pagination();
            pagination.CurrentPage = currentPage ?? 1;
            pagination.PageSize = pageSize ?? 10;
            try
            {
                var userClaims = User as ClaimsPrincipal;
                var roleClaim = userClaims.FindFirstValue(ClaimTypes.Role);
                var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                long id = long.Parse(claimsIdentity.Claims.FirstOrDefault(a => a.Type == "Id")?.Value);

                List<InvoiceDTO> result = null;
                if (roleClaim == Role.MANAGER)
                {
                    result = _invoiceService.GetAllLatestInvoice(roomCode, status ?? -1, paidDate, userId ?? -1, id, ref pagination);
                } else if(roleClaim == Role.RESIDENT)
                {
                    result = _invoiceService.GetAllLatestInvoice(roomCode, status ?? -1, paidDate, id, -1, ref pagination);
                }

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
