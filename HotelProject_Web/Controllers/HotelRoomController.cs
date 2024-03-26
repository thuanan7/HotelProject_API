using AutoMapper;
using HotelProject_Web.Models;
using HotelProject_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using HotelProject_Web.Models.VM;
using HotelProject_Web.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using HotelProject_Utility;

namespace HotelProject_Web.Controllers
{
    public class HotelRoomController : Controller
    {
        private readonly IHotelRoomService _hotelRoomService;
        private readonly IHotelService _hotelService;
        private readonly IMapper _mapper;
        public HotelRoomController(IHotelRoomService hotelRoomService, IMapper mapper, IHotelService hotelService)
        {
            _hotelRoomService = hotelRoomService;
            _mapper = mapper;
            _hotelService = hotelService;
        }
        public async Task<IActionResult> IndexHotelRoom()
        {
            List<HotelRoomDTO> list = new();
            var response = await _hotelRoomService.GetALlAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<HotelRoomDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        public async Task<IActionResult> CreateHotelRoom()
        {
            HotelRoomCreateVM hotelRoomVM = new();
            var response = await _hotelService.GetALlAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                hotelRoomVM.HotelList = JsonConvert.DeserializeObject<List<HotelDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString(),
                    });
            }
            return View(hotelRoomVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHotelRoom(HotelRoomCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _hotelRoomService.CreateAsync<APIResponse>(model.CreateHotelRoom);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Hotel created successfully";
                    return RedirectToAction(nameof(IndexHotelRoom));
                }
                else
                {
                    if (response.ErrorMessage != null)
                    {
                        TempData["error"] = "Error: Something Wrong!";
                        ModelState.AddModelError("ErrorMessage", response.ErrorMessage);
                    }
                }
            }


            var res = await _hotelService.GetALlAsync<APIResponse>();
            if (res != null && res.IsSuccess)
            {
                model.HotelList = JsonConvert.DeserializeObject<List<HotelDTO>>
                    (Convert.ToString(res.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString(),
                    });
            }
            return View(model);
        }

        public async Task<IActionResult> UpdateHotelRoom(int hotelId, int roomNo)
        {
            HotelRoomUpdateVM hotelRoomVM = new();
            var response = await _hotelRoomService.GetAsync<APIResponse>(hotelId, roomNo);
            if (response != null && response.IsSuccess)
            {
                HotelRoomDTO model = JsonConvert.DeserializeObject<HotelRoomDTO>(Convert.ToString(response.Result));
                hotelRoomVM.UpdateHotelRoom = _mapper.Map<UpdateHotelRoomDTO>(model);
            }

            
            var hotelListResponse = await _hotelService.GetALlAsync<APIResponse>();
            if (hotelListResponse != null && hotelListResponse.IsSuccess)
            {
                hotelRoomVM.HotelList = JsonConvert.DeserializeObject<List<HotelDTO>>
                    (Convert.ToString(hotelListResponse.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString(),
                    });
                return View(hotelRoomVM);
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateHotelRoom(HotelRoomUpdateVM model)
        {

            if (ModelState.IsValid)
            {
                var response = await _hotelRoomService.UpdateAsync<APIResponse>(model.UpdateHotelRoom);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Hotel updated successfully";
                    return RedirectToAction(nameof(IndexHotelRoom));
                }
                else
                {
                    if (response.ErrorMessage != null)
                    {
                        TempData["error"] = "Error: Something Wrong!";
                        ModelState.AddModelError("ErrorMessage", response.ErrorMessage);
                    }
                }
            }


            var res = await _hotelService.GetALlAsync<APIResponse>();
            if (res != null && res.IsSuccess)
            {
                model.HotelList = JsonConvert.DeserializeObject<List<HotelDTO>>
                    (Convert.ToString(res.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString(),
                    });
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteHotelRoom(int hotelId, int roomNo)
        {
            HotelRoomDeleteVM hotelRoomVM = new();
            var response = await _hotelRoomService.GetAsync<APIResponse>(hotelId, roomNo);
            if (response != null && response.IsSuccess)
            {
                hotelRoomVM.DeleteHotelRoom = JsonConvert.DeserializeObject<HotelRoomDTO>(Convert.ToString(response.Result));
            }


            var hotelListResponse = await _hotelService.GetALlAsync<APIResponse>();
            if (hotelListResponse != null && hotelListResponse.IsSuccess)
            {
                hotelRoomVM.HotelList = JsonConvert.DeserializeObject<List<HotelDTO>>
                    (Convert.ToString(hotelListResponse.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString(),
                    });
                return View(hotelRoomVM);
            }
            TempData["error"] = "Error: Something Wrong!";
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHotelRoom(HotelRoomDeleteVM model)
        {
            var response = await _hotelRoomService.DeleteAsync<APIResponse>(model.DeleteHotelRoom.HotelId, model.DeleteHotelRoom.RoomNo);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Hotel deleted successfully";
                return RedirectToAction(nameof(IndexHotelRoom));
            }
            TempData["error"] = "Error: Something Wrong!";
            return View(model);
        }
    }
}
