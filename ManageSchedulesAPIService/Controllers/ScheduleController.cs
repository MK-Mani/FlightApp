using ManageSchedulesAPIService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedServices.Models;
using System;
using System.Collections.Generic;

namespace ManageSchedulesAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ScheduleController : ControllerBase
    {
        private IScheduleRepository _scheduleRepository;

        public ScheduleController(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        // GET api/values
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            try
            {
                return Ok(_scheduleRepository.GetAllSchedules());
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Error in Get Schedules -" + ex.Message
                });
            }
        }

        // GET api/values/5
        [HttpPost]
        [Route("Search")]
        public IActionResult GetSchedule([FromBody] Schedule searchFilter)
        {
            try
            {
                return Ok(_scheduleRepository.GetSchedulesByFilter(searchFilter));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Error in Get Flight Search -" + ex.Message
                });
            }
        }

        // POST api/values
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddSchedule([FromBody] Schedule flightSchedules)
        {
            try
            {
                return Ok(_scheduleRepository.AddSchedule(flightSchedules));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Error in Add Schedule -" + ex.Message
                });
            }
        }
    }
}
