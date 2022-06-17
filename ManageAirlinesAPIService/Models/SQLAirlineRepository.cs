using SharedServices.Models;
using System.Collections.Generic;

namespace ManageAirlinesAPIService.Models
{
    public class SQLAirlineRepository : IAirlineRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLAirlineRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Airline> GetAirlines()
        {
            return _dbContext.Airlines;
        }

        public Airline GetAirlineById(int id)
        {
            return _dbContext.Airlines.Find(id);
        }

        public Airline RegisterAirline(Airline airline)
        {
            _dbContext.Airlines.Add(airline);
            _dbContext.SaveChanges();
            return airline;
        }

        public Airline UpdateDiscount(Airline airlineChanges)
        {
            var airline = _dbContext.Airlines.Find(airlineChanges.AirlineRecId);

            if(airline != null)
            {
                airline.DiscountAmount = airlineChanges.DiscountAmount;
                airline.DiscountCode = airlineChanges.DiscountCode;
                _dbContext.Entry(airline).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _dbContext.SaveChanges();
            }
            
            return airline;
        }

        public Airline BlockAirline(Airline airlineChanges)
        {
            var airline = _dbContext.Airlines.Find(airlineChanges.AirlineRecId);

            if (airline != null)
            {
                airline.IsBlock = airlineChanges.IsBlock;
                _dbContext.Entry(airline).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _dbContext.SaveChanges();
            }

            return airline;
        }
    }
}
