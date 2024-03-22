using AutoMapper;
using HotelProject_Web.DTO;
using HotelProject_Web.Models;
using HotelProject_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HotelProject_Web.Controllers
{
    public class HotelRoomController : Controller
    {
        private readonly IHotelRoomService _hotelService;
        private readonly IMapper _mapper;
        public HotelRoomController(IHotelRoomService hotelService, IMapper mapper)
        {
            _hotelService = hotelService;
            _mapper = mapper;
        }
        public async Task<IActionResult> IndexHotelRoom()
        {
            List<HotelRoomDTO> list = new();
            var response = await _hotelService.GetALlAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<HotelRoomDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
    }
}
