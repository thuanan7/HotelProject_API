using System.ComponentModel.DataAnnotations;

namespace HotelProject_HotelAPI.DTO
{
    public class HotelDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        public int Occupancy { get; set; }
        public int Size { get; set; }
    }
}
