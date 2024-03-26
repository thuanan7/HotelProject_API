using AutoMapper;
using HotelProject_Utility;
using HotelProject_Web.Models;
using HotelProject_Web.Models.DTO;
using HotelProject_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HotelProject_Web.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelService _hotelService;
        private readonly IMapper _mapper;
        public HotelController(IHotelService hotelService, IMapper mapper)
        {
            _hotelService = hotelService;
            _mapper = mapper;
        }

        public async Task<IActionResult> IndexHotel()
        {
            List<HotelDTO> list = new();
            var response = await _hotelService.GetALlAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<HotelDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateHotel()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHotel(CreateHotelDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _hotelService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Hotel created successfully";
                    return RedirectToAction(nameof(IndexHotel));
                }
                else
                {
                    if (response.ErrorMessage != null)
                    {
                        TempData["error"] = response.ErrorMessage;
                        ModelState.AddModelError("ErrorMessage", response.ErrorMessage);
                        return View(model);
                    }
                }
            }
            TempData["error"] = "Error: Something Wrong!";
            return View(model);
        }

        public async Task<IActionResult> UpdateHotel(int HotelId)
        {
            var response = await _hotelService.GetAsync<APIResponse>(HotelId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                UpdateHotelDTO model = JsonConvert.DeserializeObject<UpdateHotelDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            TempData["error"] = "Error: Something Wrong!";
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateHotel(UpdateHotelDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _hotelService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Hotel Updated successfully";
                    return RedirectToAction(nameof(IndexHotel));
                }
                else
                {
                    if (response.ErrorMessage != null)
                    {
                        TempData["error"] = response.ErrorMessage;
                        ModelState.AddModelError("ErrorMessage", response.ErrorMessage);
                        return View(model);
                    }
                }
            }
            TempData["error"] = "Error: Something Wrong!";
            return View(model);
        }

        public async Task<IActionResult> DeleteHotel(int HotelId)
        {
            var response = await _hotelService.GetAsync<APIResponse>(HotelId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                HotelDTO model = JsonConvert.DeserializeObject<HotelDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            TempData["error"] = "Error: Something Wrong!";
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHotel(HotelDTO model)
        {

            var response = await _hotelService.DeleteAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Hotel deleted successfully";
                return RedirectToAction(nameof(IndexHotel));
            }
            TempData["error"] = "Error: Something Wrong!";
            return View(model);
        }
    }
}
