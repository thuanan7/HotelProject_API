using HotelProject_HotelAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelProject_HotelAPI.Models.DTO
{
    public class CreateHotelRoomDTO
    {
        [Required]
        [Range(1, 1000, ErrorMessage = "HotelNo not valid")]
        public int RoomNo { get; set; }
        [Required]
        public int HotelId { get; set; }
        public string SpeacialDetails { get; set; }
    }
}
