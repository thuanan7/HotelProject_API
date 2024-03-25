using HotelProject_Utility;
using HotelProject_Web.Models;
using HotelProject_Web.Models.DTO;
using HotelProject_Web.Services;
using HotelProject_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HotelProject_Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO obj = new();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO obj)
        {
            APIResponse response = await _authService.LoginAsync<APIResponse>(obj);
            if (response != null && response.IsSuccess)
            {
                LoginResponseDTO model = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(model.Token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u=>u.Type=="role").Value));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Session.SetString(SD.SessionToken, model.Token);
                return RedirectToAction("Index", "Home");
            }

            TempData["error"] = "Error: Something Wrong!";
            ModelState.AddModelError("ErrorMessage", response.ErrorMessage);
            obj.Password = "";
            return View(obj);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleSelectListItem = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.Role.Admin, Value=SD.Role.Admin},
                new SelectListItem{Text=SD.Role.Customer, Value=SD.Role.Customer},
            };
            ViewBag.RoleSelectListItem = roleSelectListItem;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequestDTO obj)
        {
            if (string.IsNullOrEmpty(obj.Role))
            {
                obj.Role = SD.Role.Customer;
            }
            var response = await _authService.RegisterAsync<APIResponse>(obj);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Register successfully";
                return RedirectToAction(nameof(Login));
            }


            if (response.ErrorMessage != null)
            {
                TempData["error"] = "Error: Something Wrong!";
                ModelState.AddModelError("ErrorMessage", response.ErrorMessage);
            }
            obj.Password = "";
            var roleSelectListItem = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.Role.Admin, Value=SD.Role.Admin},
                new SelectListItem{Text=SD.Role.Customer, Value=SD.Role.Customer},
            };
            ViewBag.RoleSelectListItem = roleSelectListItem;
            return View(obj);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionToken, string.Empty);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
