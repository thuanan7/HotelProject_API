using HotelProject_HotelAPI.DTO;

namespace HotelProject_HotelAPI.Data
{
    public static class HotelStore
    {
        public static List<HotelDTO> hotelList = new List<HotelDTO>
            {
                new HotelDTO{ Id = 1, Name = "a", Occupancy=3, Size=80 },
                new HotelDTO{ Id = 2, Name = "b", Occupancy=4, Size=50 },
            };
    }
}
