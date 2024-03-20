using AutoMapper;
using HotelProject_HotelAPI.DTO;
using HotelProject_HotelAPI.Models;
using HotelProject_HotelAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace HotelProject_HotelAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class HotelApiController : ControllerBase
    {
        private readonly IHotelRepository _context;
        private readonly IMapper _mapper;
        public HotelApiController(IHotelRepository context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<HotelDTO>>> GetHotels()
        {
            var hotels = await _context.GetAllAsync();
            return Ok(_mapper.Map<List<HotelDTO>>(hotels));
        }

        [HttpGet("{id:int}", Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HotelDTO>> GetHotel(int id)
        {
            var hotel = await _context.GetAsync(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<HotelDTO>(hotel));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<HotelDTO>> CreateHotel([FromBody] CreateHotelDTO CreateHotelDTO)
        {
            if (CreateHotelDTO == null)
                return BadRequest();

            if (await _context.GetAsync(u => u.Name.ToLower() == CreateHotelDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("NameUsedError", "Name Already Exists!");
                return BadRequest(ModelState);
            }

            var hotel = _mapper.Map<Hotel>(CreateHotelDTO);
            await _context.CreateAsync(hotel);
            return CreatedAtRoute("GetHotel", new { id = hotel.Id }, _mapper.Map<HotelDTO>(hotel));
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _context.GetAsync(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }
            await _context.RemoveAsync(hotel);
            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] HotelDTO hotelDTO)
        {
            if (hotelDTO == null || hotelDTO.Id != id)
                return BadRequest();

            var hotel = await _context.GetAsync(h => h.Id == id, tracked:false);
            if (hotel == null)
                return NotFound();

            if (await _context.GetAsync(u => u.Name.ToLower() == hotelDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("NameUsedError", "Name Already Exists!");
                return BadRequest(ModelState);
            }

            var model = _mapper.Map<Hotel>(hotelDTO);
            await _context.UpdateAsync(model);
            return NoContent();
        }
    }
}
