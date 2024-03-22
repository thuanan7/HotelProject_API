using AutoMapper;
using HotelProject_Web.DTO;
using HotelProject_Web.Models;
using HotelProject_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using HotelProject_Web.Models.VM;

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHotelRoom(HotelRoomCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _hotelRoomService.CreateAsync<APIResponse>(model.CreateHotelRoom);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexHotelRoom));
                }
                else
                {
                    if (response.ErrorMessage != null)
                    {
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

        //public async Task<IActionResult> UpdateRoomHotel(int HotelId)
        //{
        //    var response = await _hotelRoomService.GetAsync<APIResponse>(HotelId);
        //    if (response != null && response.IsSuccess)
        //    {
        //        HotelDTO model = JsonConvert.DeserializeObject<HotelDTO>(Convert.ToString(response.Result));
        //        return View(model);
        //    }
        //    return NotFound();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateRoomHotel(HotelRoomDTO model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var response = await _hotelRoomService.UpdateAsync<APIResponse>(model);
        //        if (response != null && response.IsSuccess)
        //        {
        //            return RedirectToAction(nameof(IndexHotelRoom));
        //        }
        //    }
        //    return View(model);
        //}

        //public async Task<IActionResult> DeleteHotelRoom(int HotelId)
        //{
        //    var response = await _hotelRoomService.GetAsync<APIResponse>(HotelId);
        //    if (response != null && response.IsSuccess)
        //    {
        //        HotelDTO model = JsonConvert.DeserializeObject<HotelDTO>(Convert.ToString(response.Result));
        //        return View(model);
        //    }
        //    return NotFound();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteHotelRoom(HotelRoomDTO model)
        //{

        //    var response = await _hotelRoomService.DeleteAsync<APIResponse>(model.Id);
        //    if (response != null && response.IsSuccess)
        //    {
        //        return RedirectToAction(nameof(IndexHotel));
        //    }
        //    return View(model);
        //}
    }
}
