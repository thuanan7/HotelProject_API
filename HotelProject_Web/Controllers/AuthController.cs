using HotelProject_Web.Models;
using HotelProject_Web.Models.DTO;
using HotelProject_Web.Services;
using HotelProject_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;

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
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequestDTO obj)
        {
            var response = await _authService.RegisterAsync<APIResponse>(obj);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Hotel created successfully";
                return RedirectToAction(nameof(Login));
            }
            else
            {
                if (response.ErrorMessage != null)
                {
                    TempData["error"] = "Error: Something Wrong!";
                    ModelState.AddModelError("ErrorMessage", response.ErrorMessage);
                }
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
