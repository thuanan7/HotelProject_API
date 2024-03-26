using HotelProject_Utility;
using HotelProject_Web.Models;
using HotelProject_Web.Models.DTO;
using HotelProject_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HotelProject_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHotelService _hotelService;
        public HomeController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public async Task<IActionResult> Index()
        {
            List<HotelDTO> list = new();
            var response = await _hotelService.GetALlAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<HotelDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

    }
}
