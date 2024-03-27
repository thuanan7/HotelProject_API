using AutoMapper;
using HotelProject_HotelAPI.Data;
using HotelProject_HotelAPI.Models;
using HotelProject_HotelAPI.Models.DTO;
using HotelProject_HotelAPI.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelProject_HotelAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly string secretKey;
        private readonly int _accessTokenTimeExpires = 1;
        private readonly int _refreshTokenTimeExpires = 2;
        public UserRepository(ApplicationDbContext context, IMapper mapper, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _mapper = mapper;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public bool IsUniqueUser(string username)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<TokenDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if (user == null || !isValid)
            {
                return new TokenDTO()
                {
                    AccessToken = "",
                };
            }

            var jwtTokenId = $"JTI{Guid.NewGuid()}";
            var refreshToken = await CreateNewRefreshToken(user.Id, jwtTokenId);
            TokenDTO loginResponseDTO = new()
            {
                AccessToken = await GetAccessToken(user, jwtTokenId),
                RefreshToken = refreshToken
            };
            return loginResponseDTO;
        }

        public async Task<RegisterResponseDTO> Register(RegisterRequestDTO registerRequestDTO)
        {
            ApplicationUser user = new()
            {
                UserName = registerRequestDTO.UserName,
                Email = registerRequestDTO.UserName,
                NormalizedEmail = registerRequestDTO.UserName.ToUpper(),
                Name = registerRequestDTO.Name,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registerRequestDTO.Password);
                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(registerRequestDTO.Role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(registerRequestDTO.Role));
                    }

                    await _userManager.AddToRoleAsync(user, registerRequestDTO.Role);
                    var userToReturn = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == registerRequestDTO.UserName);
                    return new RegisterResponseDTO
                    {
                        User = _mapper.Map<UserDTO>(userToReturn)
                    };
                }
                else
                {
                    var res = new RegisterResponseDTO
                    {
                        User = new UserDTO(),
                        ErrorMessage = result.Errors.FirstOrDefault().Description
                    };
                    return res;
                }
            }
            catch (Exception ex)
            {
                return new RegisterResponseDTO
                {
                    User = new UserDTO(),
                    ErrorMessage = ex.Message
                };
            }
        }

        private async Task<string> GetAccessToken(ApplicationUser user, string jwtTokenId)
        {
            // gererate JWT Token
            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandle = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                    new Claim(JwtRegisteredClaimNames.Jti, jwtTokenId),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_accessTokenTimeExpires),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandle.CreateToken(tokenDescriptor);
            var tokenStr = tokenHandle.WriteToken(token);
            return tokenStr;
        }

        public async Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO)
        {
            // Find an existing refresh token
            var existingRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(u => u.Refresh_Token == tokenDTO.RefreshToken);
            if (existingRefreshToken == null)
            {
                return new TokenDTO();
            }

            // Compare data from 'existing refresh' and 'access token provided'
            var isTokenValid = CheckAccessTokenData(tokenDTO.AccessToken, existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
            if (!isTokenValid)
            {
                await MarkTokenAsInvalid(existingRefreshToken);
                return new TokenDTO();
            }

            // When someone tries to use not valid refresh token, security situation possible
            if (!existingRefreshToken.IsValid)
            {
                await MarkAllTokenInChainAsInvalid(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);

                return new TokenDTO();
            }


            // If expired then mark as invalid and return null
            if (existingRefreshToken.ExpiresAt < DateTime.UtcNow)
            {
                await MarkTokenAsInvalid(existingRefreshToken);
                return new TokenDTO();
            }

            // replace old refresh token with a new one with updated expire date
            var newRefreshToken = await CreateNewRefreshToken(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);

            // revoke existing refresh token
            existingRefreshToken.IsValid = false;
            await _context.SaveChangesAsync();

            // generate new access token and return token dto
            var applicationUser = _context.ApplicationUsers.FirstOrDefault(u => u.Id == existingRefreshToken.UserId);
            if (applicationUser == null)
            {
                return new TokenDTO();
            }

            var newAccessToken = await GetAccessToken(applicationUser, existingRefreshToken.JwtTokenId);

            return new TokenDTO()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            };
        }

        private async Task<string> CreateNewRefreshToken(string userId, string tokenId)
        {
            RefreshToken refreshToken = new()
            {
                IsValid = true,
                UserId = userId,
                JwtTokenId = tokenId,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_refreshTokenTimeExpires),
                Refresh_Token = Guid.NewGuid() + "-" + Guid.NewGuid(),
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken.Refresh_Token;
        }

        private bool CheckAccessTokenData(string accessToken, string expectedUserId,
            string expectedTokenId)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwt = tokenHandler.ReadJwtToken(accessToken);
                var jwtTokenId = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Jti).Value;
                var userId = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value;

                return userId == expectedUserId && jwtTokenId == expectedTokenId;
            }
            catch
            {
                return false;
            }
        }

        private async Task MarkAllTokenInChainAsInvalid(string userId, string tokenId)
        {
            await _context.RefreshTokens.Where(u => u.UserId == userId && u.JwtTokenId == tokenId)
                    .ExecuteUpdateAsync(u => u.SetProperty(refreshToken => refreshToken.IsValid, false));
        }

        private Task MarkTokenAsInvalid(RefreshToken refreshToken)
        {
            refreshToken.IsValid = false;
            return _context.SaveChangesAsync();
        }
    }
}
