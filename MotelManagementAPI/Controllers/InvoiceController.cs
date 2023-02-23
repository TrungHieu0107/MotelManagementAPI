using BussinessObject.DTO.Common;
using DataAccess.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MotelManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

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

        [HttpGet]
        [Route("get-unpay-invocie/{roomCode}")]
        public IActionResult GetUnpayInvocie(string roomCode)
        {
            CommonResponse commonResponse = new CommonResponse();
            try
            {
                var invocie = _invoiceService.checkUnPayInvocieByRoomCode(roomCode);
              
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
                    commonResponse.Message = "Some thing went wrong, plase check the data again";
                    return BadRequest(commonResponse);
                }
                else
                {
                    commonResponse.Message = " Pay Successfully";
                    return Ok(commonResponse);

                }
              
            }
            catch (Exception ex)
            {
                commonResponse.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, commonResponse);
            }


        }
    }
}
