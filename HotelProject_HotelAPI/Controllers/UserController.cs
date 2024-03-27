using Asp.Versioning;
using HotelProject_HotelAPI.Models;
using HotelProject_HotelAPI.Models.DTO;
using HotelProject_HotelAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace HotelProject_HotelAPI.Controllers
{
    [ApiVersionNeutral]
    [Route("/api/[Controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private APIResponse _response;
        public UserController(IUserRepository userRepostiroy)
        {
            _userRepository = userRepostiroy;
            _response = new APIResponse();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var tokenDTO = await _userRepository.Login(model);
            if (tokenDTO == null || string.IsNullOrEmpty(tokenDTO.AccessToken))
            {
                _response.ErrorMessage = "Username or password is incorrect";
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.Result = tokenDTO;
            return Ok(_response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            bool isUserNameUnique = _userRepository.IsUniqueUser(model.UserName);
            if (!isUserNameUnique)
            {
                _response.ErrorMessage = "Username Already Exists!";
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            var registerResponse = await _userRepository.Register(model);
            if (!string.IsNullOrEmpty(registerResponse.ErrorMessage))
            {
                _response.ErrorMessage = registerResponse.ErrorMessage;
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var user = registerResponse.User;
            if (user.ID == null)
            {
                _response.ErrorMessage = "Error while registering!";
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.Result = user;
            return Ok(_response);
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] TokenDTO tokenDTO)
        {
            if (ModelState.IsValid)
            {
                await _userRepository.RevokeRefreshToken(tokenDTO);
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            _response.IsSuccess = false;
            _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            _response.ErrorMessage = "Invalid Input";
            return BadRequest(_response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> GetNewTokenFromRefreshToken([FromBody] TokenDTO tokenDTO)
        {
            if (ModelState.IsValid)
            {
                var tokenDTOResponse = await _userRepository.RefreshAccessToken(tokenDTO);
                if (tokenDTOResponse == null || string.IsNullOrEmpty(tokenDTOResponse.AccessToken))
                {
                    _response.ErrorMessage = "Token Invalid";
                    _response.IsSuccess = false;
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = tokenDTOResponse;
                return Ok(_response);
            }
            else
            {
                _response.ErrorMessage = "Invalid Input";
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

        }
    }
}
