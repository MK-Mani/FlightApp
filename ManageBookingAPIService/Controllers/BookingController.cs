using ManageBookingAPIService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedServices.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ManageBookingAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BookingController : ControllerBase
    {
        private IBookingRepository _bookingRepository;
        public BookingController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Manage", "Bookings" };
        }


        [HttpPost]
        [Route("Book")]
        public IActionResult BookTicket([FromBody] Booking ticketDetails)
        {
            try
            {
                var isbooked = _bookingRepository.BookTicket(ticketDetails);

                return Ok(new ApiResponse()
                {
                    StatusCode = isbooked ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable,
                    Message = isbooked ? "Your ticket has been booked successfully." : "Your ticket has not been booked successfully"
                }); 
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Error in Book Ticket -" + ex.Message
                });
            }
        }

        [HttpGet]
        [Route("Cancel")]
        public IActionResult CacelTicket(int bookingRecId)
        {
            try
            {
                var isCancel = _bookingRepository.CancelBookedTicket(bookingRecId);

                return Ok(new ApiResponse()
                {
                    StatusCode = isCancel ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable,
                    Message = isCancel ? "Your ticket has been canceled" : "Your ticket has not been canceled"
                }) ;
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Error in Cancel Ticket -" + ex.Message
                });
            }
        }

        [HttpGet]
        [Route("GetBookedTicket")]
        public IActionResult GetBookedTicket(int pnrNumber)
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(_bookingRepository.GetBookedTicket(pnrNumber), new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Error in Getting booking Ticket -" + ex.Message
                });
            }            
        }

        [HttpGet]
        [Route("GetBookingHistory")]
        public IActionResult GetBookingHistory(string emailId)
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(_bookingRepository.GetBookingHistory(emailId), new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Error in Booking History -" + ex.Message
                });
            }            
        }

        //[HttpGet]
        //[Route("Download")]
        //public ActionResult Get(string reportName)
        //{
        //    _bookingRepository.DownloadReprt();
        //    var returnString = _reportService.GenerateReportAsync(reportName);
        //    return File(returnString, System.Net.Mime.MediaTypeNames.Application.Octet, reportName + ".pdf");
        //}
    }
}
