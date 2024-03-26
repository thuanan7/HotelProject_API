using System.ComponentModel.DataAnnotations;

namespace HotelProject_HotelAPI.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string JwtTokenId { get; set; }
        public string Refresh_Token { get; set; }
        // Only valid for one use
        public bool IsValid { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
