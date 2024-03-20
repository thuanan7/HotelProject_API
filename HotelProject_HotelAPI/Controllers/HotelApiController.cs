using HotelProject_HotelAPI.DTO;
using HotelProject_HotelAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace HotelProject_HotelAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class HotelApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public HotelApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<HotelDTO>> GetHotels()
        {
            return Ok(_context.Hotels);
        }

        [HttpGet("{id:int}", Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<HotelDTO> GetHotel(int id)
        {
            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }
            return Ok(hotel);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<HotelDTO> CreateHotel([FromBody] HotelDTO hotelDTO)
        {
            if (hotelDTO == null || hotelDTO.Id != 0)
                return BadRequest();

            if (_context.Hotels.FirstOrDefault(u => u.Name.ToLower() == hotelDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("NameUsedError", "Name Already Exists!");
                return BadRequest(ModelState);
            }


            var hotel = new Hotel
            {
                Name = hotelDTO.Name,
                Description = hotelDTO.Description,
                Occupancy = hotelDTO.Occupancy,
                Size = hotelDTO.Size,
                Price = hotelDTO.Price,
                Amenity = hotelDTO.Amenity,
            };
            _context.Hotels.Add(hotel);
            _context.SaveChanges();
            hotelDTO.Id = hotel.Id;
            return CreatedAtRoute("GetHotel", new { id = hotel.Id }, hotelDTO);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteHotel(int id)
        {
            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }
            _context.Hotels.Remove(hotel);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateHotel(int id, [FromBody] HotelDTO hotelDTO)
        {
            if (hotelDTO == null || hotelDTO.Id != id)
                return BadRequest();

            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
                return NotFound();

            if (_context.Hotels.FirstOrDefault(u => u.Name.ToLower() == hotelDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("NameUsedError", "Name Already Exists!");
                return BadRequest(ModelState);
            }

            hotel.Name = hotelDTO.Name;
            hotel.Description = hotelDTO.Description;
            hotel.Occupancy = hotelDTO.Occupancy;
            hotel.Size = hotelDTO.Size;
            hotel.Price = hotelDTO.Price;
            hotel.Amenity = hotelDTO.Amenity;
            _context.SaveChanges();
            return NoContent();
        }
    }
}
