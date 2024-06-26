﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelProject_Web.Models.DTO
{
    public class HotelRoomDTO
    {
        [Required]
        [Range(1, 1000, ErrorMessage = "HotelNo not valid")]
        public int RoomNo { get; set; }
        [Required]
        public int HotelId { get; set; }
        public string SpeacialDetails { get; set; }
        public HotelDTO Hotel { get; set; }
    }
}
