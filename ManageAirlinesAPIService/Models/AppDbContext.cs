using Microsoft.EntityFrameworkCore;
using SharedServices.Models;

namespace ManageAirlinesAPIService.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Airline> Airlines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Airline>().Property(x => x.IsActive).IsRequired(true);
            //modelBuilder.Entity<Airline>().Property(x => x.DiscountCode).IsRequired(true);
            //modelBuilder.Entity<Airline>().Property(x => x.DiscountAmount).IsRequired(true);
        }
    }

}
