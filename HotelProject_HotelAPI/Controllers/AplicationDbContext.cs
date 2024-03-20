using HotelProject_HotelAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelProject_HotelAPI.Controllers
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Royal Hotel",
                    Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa3.jpg",
                    Occupancy = 4,
                    Price = 200,
                    Size = 550,
                    Amenity = "",
                    CreatedDate = DateTime.Now,
                },
              new Hotel
              {
                  Id = 2,
                  Name = "Premium Pool Hotel",
                  Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa1.jpg",
                  Occupancy = 4,
                  Price = 300,
                  Size = 550,
                  Amenity = "",
                  CreatedDate = DateTime.Now,
              },
              new Hotel
              {
                  Id = 3,
                  Name = "Luxury Pool Hotel",
                  Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa4.jpg",
                  Occupancy = 4,
                  Price = 400,
                  Size = 750,
                  Amenity = "",
                  CreatedDate = DateTime.Now,
              },
              new Hotel
              {
                  Id = 4,
                  Name = "Diamond Hotel",
                  Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa5.jpg",
                  Occupancy = 4,
                  Price = 550,
                  Size = 900,
                  Amenity = "",
                  CreatedDate = DateTime.Now,
              },
              new Hotel
              {
                  Id = 5,
                  Name = "Diamond Pool Hotel",
                  Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa2.jpg",
                  Occupancy = 4,
                  Price = 600,
                  Size = 1100,
                  Amenity = "",
                  CreatedDate = DateTime.Now,
              }
              );
        }
    }
}
