using SharedServices.Models;
using System.Collections.Generic;

namespace ManageAirlinesAPIService.Models
{
    public interface IAirlineRepository
    {
        IEnumerable<Airline> GetAirlines();

        Airline GetAirlineById(int id);

        Airline RegisterAirline(Airline airline);

        Airline UpdateDiscount(Airline airline);

        Airline BlockAirline(Airline airline);
    }
}
