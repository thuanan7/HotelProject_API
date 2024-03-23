using AutoMapper;
using Azure;
using HotelProject_HotelAPI.Models;
using HotelProject_HotelAPI.Models.DTO;
using HotelProject_HotelAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Xml.Linq;

namespace HotelProject_HotelAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class HotelApiController : ControllerBase
    {
        private readonly IHotelRepository _context;
        private readonly IMapper _mapper;
        private APIResponse _response;
        public HotelApiController(IHotelRepository context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _response = new APIResponse();
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> GetHotels()
        {
            try
            {
                var hotels = await _context.GetAllAsync();
                _response.Result = _mapper.Map<List<HotelDTO>>(hotels);
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

        [HttpGet("{id:int}", Name = "GetHotel")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetHotel(int id)
        {
            try
            {
                var hotel = await _context.GetAsync(h => h.Id == id);
                if (hotel == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessage = "Not Found";
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<HotelDTO>(hotel);
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
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> CreateHotel([FromBody] CreateHotelDTO CreateHotelDTO)
        {
            if (CreateHotelDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessage = "Bad Request";
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            if (await _context.GetAsync(u => u.Name.ToLower() == CreateHotelDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("NameUsedError", "Name Already Exists!");
                return BadRequest(ModelState);
            }

            var hotel = _mapper.Map<Hotel>(CreateHotelDTO);
            hotel.CreatedDate = DateTime.Now;
            await _context.CreateAsync(hotel);
            
            _response.StatusCode = HttpStatusCode.Created;
            _response.Result = _mapper.Map<HotelDTO>(hotel);
            return CreatedAtRoute("GetHotel", new { id = hotel.Id }, _response);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> DeleteHotel(int id)
        {
            var hotel = await _context.GetAsync(h => h.Id == id);
            if (hotel == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessage = "Not Found";
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            await _context.RemoveAsync(hotel);
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "custom")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> UpdateHotel(int id, [FromBody] HotelDTO hotelDTO)
        {
            if (hotelDTO == null || hotelDTO.Id != id)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessage = "Bad Request";
                _response.IsSuccess=false;
                return BadRequest(_response);
            }

            var hotel = await _context.GetAsync(h => h.Id == id, tracked: false);
            if (hotel == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessage = "Not Found";
                _response.IsSuccess = false;
                return NotFound(_response);
            }

            if (await _context.GetAsync(u => u.Id != id && u.Name.ToLower() == hotelDTO.Name.ToLower()) != null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessage = "Name Already Exists!";
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            var model = _mapper.Map<Hotel>(hotelDTO);
            await _context.UpdateAsync(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}
