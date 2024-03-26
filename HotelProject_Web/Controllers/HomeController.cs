using AutoMapper;
using HotelProject_Utility;
using HotelProject_Web.Models;
using HotelProject_Web.Models.DTO;
using HotelProject_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace HotelProject_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHotelService _hotelService;
        private readonly IMapper _mapper;
        public HomeController(IHotelService hotelService, IMapper mapper)
        {
            _hotelService = hotelService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<HotelDTO> list = new();
            var response = await _hotelService.GetALlAsync<APIResponse>(HttpContext.Session.GetString(SD.AccessToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<HotelDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

    }
}
