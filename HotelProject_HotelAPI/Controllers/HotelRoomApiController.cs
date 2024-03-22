using AutoMapper;
using Azure;
using HotelProject_HotelAPI.DTO;
using HotelProject_HotelAPI.Models;
using HotelProject_HotelAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Xml.Linq;

namespace HotelProject_HotelAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class HotelRoomApiController : ControllerBase
    {
        private readonly IHotelRoomRepository _context;
        private readonly IHotelRepository _hoteldb;
        private readonly IMapper _mapper;
        private APIResponse _response;
        public HotelRoomApiController(IHotelRoomRepository context, IMapper mapper, IHotelRepository hoteldb)
        {
            _context = context;
            _mapper = mapper;
            _response = new APIResponse();
            _hoteldb = hoteldb;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetHotelRooms()
        {
            try
            {
                var hotelRooms = await _context.GetAllAsync(includeProperties:"Hotel");
                _response.Result = _mapper.Map<List<HotelRoomDTO>>(hotelRooms);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return _response;
            }
        }

        [HttpGet("{roomNo:int}", Name = "GetHotelRoom")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetHotelRoom(int roomNo)
        {
            try
            {
                var hotelRoom = await _context.GetAsync(h => h.RoomNo == roomNo);
                if (hotelRoom == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessage = "Not Found";
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<HotelRoomDTO>(hotelRoom);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return _response;
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateHotelRoom([FromBody] CreateHotelRoomDTO createHotelRoomDTO)
        {
            if (createHotelRoomDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessage = "Bad Request";
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            if (await _hoteldb.GetAsync(h => h.Id == createHotelRoomDTO.HotelId) == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessage = "Hotel Doesn't Exits!";
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            var h = await _context.GetAsync(u => u.HotelId == createHotelRoomDTO.HotelId && u.RoomNo == createHotelRoomDTO.RoomNo, tracked: false, includeProperties: "Hotel");
            if (h != null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessage = $"Room {h.RoomNo} Already Exists in {h.Hotel.Name}!";
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            var hotelRoom = _mapper.Map<HotelRoom>(createHotelRoomDTO);
            hotelRoom.CreatedDate = DateTime.Now;
            await _context.CreateAsync(hotelRoom);

            _response.StatusCode = HttpStatusCode.Created;
            _response.Result = _mapper.Map<HotelRoomDTO>(hotelRoom);
            return CreatedAtRoute("GetHotelRoom", new { roomNo = hotelRoom.RoomNo }, _response);
        }

        [HttpDelete("hotels/{hotelId:int}/rooms/{roomNo}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> DeleteHotelRoom(int hotelId, int roomNo)
        {
            var hotelRoom = await _context.GetAsync(r => r.HotelId == hotelId && r.RoomNo == roomNo);
            if (hotelRoom == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessage = "Not Found";
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            await _context.RemoveAsync(hotelRoom);
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }

        [HttpPut("hotels/{hotelId:int}/rooms/{roomNo}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> UpdateHotelRoom(int hotelId, int roomNo, [FromBody] UpdateHotelRoomDTO updateHotelRoomDTO)
        {
            if (updateHotelRoomDTO == null || updateHotelRoomDTO.HotelId != hotelId || updateHotelRoomDTO.RoomNo != roomNo)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessage = "Bad Request";
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            var hotelRoom = await _context.GetAsync(r => r.HotelId == hotelId && r.RoomNo == roomNo, tracked: false);
            if (hotelRoom == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessage = "Not Found";
                _response.IsSuccess = false;
                return NotFound(_response);
            }

            var model = _mapper.Map<HotelRoom>(updateHotelRoomDTO);
            await _context.UpdateAsync(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}
