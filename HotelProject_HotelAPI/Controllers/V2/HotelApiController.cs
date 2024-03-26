using Asp.Versioning;
using AutoMapper;
using HotelProject_HotelAPI.Models;
using HotelProject_HotelAPI.Models.DTO;
using HotelProject_HotelAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace HotelProject_HotelAPI.Controllers.V2
{
    [Route("/api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
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
        [ResponseCache(Duration = 30)]
        public async Task<ActionResult<APIResponse>> GetHotels([FromQuery(Name = "Filter Occupancy")] int? occupancy,
            [FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                IEnumerable<Hotel> hotels;
                if (occupancy > 0)
                {
                    //Filter at db
                    hotels = await _context.GetAllAsync(h => h.Occupancy == occupancy, pageSize: pageSize, pageNumber: pageNumber);
                }
                else
                {
                    hotels = await _context.GetAllAsync(pageSize: pageSize, pageNumber: pageNumber);
                }

                //Filter at back-end server after query data from db
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim().ToLower();
                    hotels = hotels.Where(h => h.Name.ToLower().Contains(search) || h.Amenity.ToLower().Contains(search));
                }

                Pagination pagination = new Pagination() { PageNubmer = pageNumber, PageSize = pageSize };
                Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseCache(CacheProfileName = "Default30")]
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
        [Authorize]
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
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> UpdateHotel(int id, [FromBody] UpdateHotelDTO hotelDTO)
        {
            if (hotelDTO == null || hotelDTO.Id != id)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessage = "Bad Request";
                _response.IsSuccess = false;
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
