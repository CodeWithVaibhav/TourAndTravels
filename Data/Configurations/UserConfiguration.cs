using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourAndTravels.Data.Entities;

namespace TourAndTravels.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.Property(s => s.UserId).UseIdentityColumn();
            builder.Property(s => s.Username).IsRequired().HasMaxLength(100);
            builder.Property(s => s.Password).IsRequired().HasMaxLength(100);
            builder.Property(s => s.Firstname).IsRequired().HasMaxLength(100);
            builder.Property(s => s.Lastname).IsRequired().HasMaxLength(100);
            builder.Property(s => s.Email).HasMaxLength(100);
            builder.Property(s => s.PhoneNumber).HasMaxLength(100);

            builder.HasData
            (
                new User
                {
                    Email = "admin@showroom.com",
                    Firstname = "Admin",
                    Lastname = "User",
                    Password = "TourAndTravels@123",
                    Username = "admin",
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now,
                    UserId = 1
                }
            );
        }
    }
}
