using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelProject_HotelAPI.Models
{
    public class Hotel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        [DataType(DataType.Currency, ErrorMessage = "Price must be valid")]
        public double Price { get; set; }
        [Range(1, 10, ErrorMessage = "Occupancy must be between 1 and 10")]
        public int Occupancy { get; set; }
        public int Size { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
        public string? Amenity { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
