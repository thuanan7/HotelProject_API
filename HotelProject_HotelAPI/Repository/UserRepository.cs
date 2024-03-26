﻿using AutoMapper;
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

namespace HotelProject_HotelAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly string secretKey;
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
            var user = _context.ApplicationUsers.FirstOrDefault(u=> u.UserName == username);
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
                    AccessToken="",
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
                    var userToReturn = _context.ApplicationUsers.FirstOrDefault(u=>u.UserName== registerRequestDTO.UserName);
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
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandle.CreateToken(tokenDescriptor);
            var tokenStr = tokenHandle.WriteToken(token);
            return tokenStr;
        }

        public Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO)
        {
            throw new NotImplementedException();
        }

        private async Task<string> CreateNewRefreshToken(string userId, string tokenId)
        {
            RefreshToken refreshToken = new()
            {
                IsValid = true,
                UserId = userId,
                JwtTokenId = tokenId,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                Refresh_Token = Guid.NewGuid() + "-" + Guid.NewGuid(),
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken.Refresh_Token;
        }

        private(bool isSuccess, string userId, string tokenId) GetAccessTokenData(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwt = tokenHandler.ReadJwtToken(accessToken);
                var jwtTokenId = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Jti).Value;
                var userId = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value;

                return(true, userId, jwtTokenId);
            }
            catch 
            {
                return (false, null, null);
            }
        }
    }
}
