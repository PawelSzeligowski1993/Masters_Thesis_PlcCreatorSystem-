using Microsoft.AspNetCore.Mvc;
using PlcCreatorSystem_WEB.Models;
using PlcCreatorSystem_WEB.Models.Dto;
using PlcCreatorSystem_WEB.Services.IServices;

namespace PlcCreatorSystem_WEB.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            LoginRequestDTO loginRequestDTO = new();
            return View(loginRequestDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            return View(loginRequestDTO);
        }

        [HttpGet]
        public IActionResult Register() 
        { 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            APIResponse result = await _authService.RegisterAsync<APIResponse>(registerationRequestDTO);
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
