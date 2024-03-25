using Microsoft.AspNetCore.Identity;

namespace HotelProject_HotelAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
