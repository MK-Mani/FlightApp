using Microsoft.EntityFrameworkCore;
using SharedServices.Models;

namespace ManageSchedulesAPIService.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<Airline> Airlines { get; set; }
    }
}
