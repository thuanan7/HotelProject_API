﻿using System.ComponentModel.DataAnnotations;

namespace HotelProject_HotelAPI.DTO
{
    public class CreateHotelDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public double Price { get; set; }
        public int Occupancy { get; set; }
        public int Size { get; set; }
        public string? ImageUrl { get; set; }
        public string? Amenity { get; set; }
    }
}
