using Microsoft.EntityFrameworkCore;
using SharedServices.Models;

namespace ManageBookingAPIService.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<Airline> Airlines { get; set; }

        public DbSet<Passenger> Passenger { get; set; }
    }
}
