using HotelProject_HotelAPI.Data;
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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<HotelDTO>> GetHotels()
        {
            return Ok(HotelStore.hotelList);
        }

        [HttpGet("{id:int}", Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<HotelDTO> GetHotel(int id)
        {
            var hotel = HotelStore.hotelList.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }
            return Ok(hotel);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<HotelDTO> CreateHotel([FromBody] HotelDTO hotel)
        {
            if (hotel == null || hotel.Id != 0)
                return BadRequest();

            if (HotelStore.hotelList.FirstOrDefault(u => u.Name.ToLower() == hotel.Name.ToLower()) != null)
            {
                ModelState.AddModelError("NameUsedError", "Name Already Exists!");
                return BadRequest(ModelState);
            }

            hotel.Id = HotelStore.hotelList.OrderByDescending(h => h.Id).FirstOrDefault().Id + 1;

            HotelStore.hotelList.Add(hotel);
            return CreatedAtRoute("GetHotel", new { id = hotel.Id }, hotel);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteHotel(int id)
        {
            var hotel = HotelStore.hotelList.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }
            HotelStore.hotelList.Remove(hotel);
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

            var hotel = HotelStore.hotelList.FirstOrDefault(h=>h.Id == id);
            if (hotel == null)
                return NotFound();

            if (HotelStore.hotelList.FirstOrDefault(u => u.Name.ToLower() == hotelDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("NameUsedError", "Name Already Exists!");
                return BadRequest(ModelState);
            }

            hotel.Name = hotelDTO.Name;
            hotel.Occupancy = hotelDTO.Occupancy;
            hotel.Size = hotelDTO.Size;
            return NoContent();
        }
    }
}
