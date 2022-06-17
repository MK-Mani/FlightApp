using ManageAirlinesAPIService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ManageAirlinesAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AirlineController : ControllerBase
    {
        private readonly IAirlineRepository _airlineRepository;

        public AirlineController(IAirlineRepository airlineRepository)
        {
            _airlineRepository = airlineRepository;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //Producer.Publish(new User
            //{
            //    UserRecId = 1,
            //    LastName = "Consumer"
            //});
            return new string[] { "Airlines", "Service" };
        }

        [HttpGet]
        [Route("Get")]
        public IEnumerable<Airline> GetAirlines()
        {
            var airlines = _airlineRepository.GetAirlines();

            if(airlines?.Count() > 0)
            {
                airlines = airlines.Where(item => item.IsBlock != true);
            }

            return airlines;
        }

        [HttpGet]
        [Route("GetById")]
        public Airline GetAirlineById(int id)
        {
            return _airlineRepository.GetAirlineById(id);
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult RegisterAirline([FromBody] Airline airline)
        {
            var newAirline = _airlineRepository.RegisterAirline(airline);

            return Ok(newAirline);
        }

        [HttpPut]
        [Route("UpdateDiscount")]
        public Airline UpdateDiscount([FromBody] Airline airline)
        {
            return _airlineRepository.UpdateDiscount(airline);
        }

        [HttpPost]
        [Route("Block")]
        public IActionResult BlockAirline(Airline airline)
        {
            return Ok(_airlineRepository.BlockAirline(airline));
        }
    }
}
