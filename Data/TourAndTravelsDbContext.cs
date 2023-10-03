using Microsoft.EntityFrameworkCore;
using TourAndTravels.Data.Configurations;
using TourAndTravels.Data.Entities;

namespace TourAndTravels.Data
{
    public class TourAndTravelsDbContext: DbContext
    {
        public DbSet<User> User { get; set; }

        public TourAndTravelsDbContext(DbContextOptions options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
