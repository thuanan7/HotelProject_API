using HotelProject_Web.Models.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelProject_Web.Models.VM
{
    public class HotelRoomCreateVM
    {
        public CreateHotelRoomDTO CreateHotelRoom { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> HotelList { get; set; }
        public HotelRoomCreateVM()
        {
            CreateHotelRoom = new();
        }
    }
}
