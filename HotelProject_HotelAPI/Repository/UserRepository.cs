using AutoMapper;
using HotelProject_HotelAPI.Data;
using HotelProject_HotelAPI.Models;
using HotelProject_HotelAPI.Models.DTO;
using HotelProject_HotelAPI.Repository.IRepository;

namespace HotelProject_HotelAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public bool IsUniqueUser(string username)
        {
            var user = _context.LocalUser.FirstOrDefault(u=> u.UserName == username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<LocalUser> Register(RegisterRequestDTO registerRequestDTO)
        {
            LocalUser user = _mapper.Map<LocalUser>(registerRequestDTO);
            await _context.LocalUser.AddAsync(user);
            await _context.SaveChangesAsync();
            user.Password = "";
            return user;
        }
    }
}
