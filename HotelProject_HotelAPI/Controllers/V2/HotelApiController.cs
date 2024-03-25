using Asp.Versioning;
using AutoMapper;
using HotelProject_HotelAPI.Models;
using HotelProject_HotelAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;


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

        [HttpGet("GetStrings")]
        public IEnumerable<string> Get()
        {
            return ["a", "b", "c"];
        }

        
    }
}
