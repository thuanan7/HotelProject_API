using HotelProject_HotelAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelProject_HotelAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<HotelRoom> HotelRooms { get; set; }
        public DbSet<LocalUser> LocalUser { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Royal Hotel",
                    Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    ImageUrl = "https://placehold.co/600x401",
                    Occupancy = 4,
                    Price = 200,
                    Size = 550,
                    Amenity = "",
                    CreatedDate = new DateTime(2024, 03, 25),
                },
              new Hotel
              {
                  Id = 2,
                  Name = "Premium Pool Hotel",
                  Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://placehold.co/600x402",
                  Occupancy = 4,
                  Price = 300,
                  Size = 550,
                  Amenity = "",
                  CreatedDate = new DateTime(2024, 03, 25),
              },
              new Hotel
              {
                  Id = 3,
                  Name = "Luxury Pool Hotel",
                  Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://placehold.co/600x403",
                  Occupancy = 4,
                  Price = 400,
                  Size = 750,
                  Amenity = "",
                  CreatedDate = new DateTime(2024, 03, 25),
              },
              new Hotel
              {
                  Id = 4,
                  Name = "Diamond Hotel",
                  Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://placehold.co/600x404",
                  Occupancy = 4,
                  Price = 550,
                  Size = 900,
                  Amenity = "",
                  CreatedDate = new DateTime(2024, 03, 25),
              },
              new Hotel
              {
                  Id = 5,
                  Name = "Diamond Pool Hotel",
                  Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://placehold.co/600x405",
                  Occupancy = 4,
                  Price = 600,
                  Size = 1100,
                  Amenity = "",
                  CreatedDate = new DateTime(2024, 03, 25),
              }
              );
        }
    }
}
