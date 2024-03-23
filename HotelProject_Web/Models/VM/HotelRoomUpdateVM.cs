using HotelProject_Web.Models.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelProject_Web.Models.VM
{
    public class HotelRoomUpdateVM
    {
        public UpdateHotelRoomDTO UpdateHotelRoom { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> HotelList { get; set; }
        public HotelRoomUpdateVM()
        {
            UpdateHotelRoom = new();
        }
    }
}
