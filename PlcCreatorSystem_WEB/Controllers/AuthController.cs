using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlcCreatorSystem_Utility;
using PlcCreatorSystem_WEB.Models;
using PlcCreatorSystem_WEB.Models.Dto;
using PlcCreatorSystem_WEB.Services.IServices;
using System.Security.Claims;

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
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new();
            return View(loginRequestDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            APIResponse response = await _authService.LoginAsync<APIResponse>(loginRequestDTO);
            if(response != null && response.IsSuccess)
            {
                LoginResponseDTO loginResponseDTO = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, loginResponseDTO.User.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, loginResponseDTO.User.Role)); //could be array of roles or loop the roles if multiple role are
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


                HttpContext.Session.SetString(SD.SessionToken, loginResponseDTO.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", response.ErrorsMessages.FirstOrDefault());
                return View(loginRequestDTO);
            }
            
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
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionToken, "");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() 
        {
            return View();
        }
    }
}
