using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PlcCreatorSystem_Utility;
using PlcCreatorSystem_WEB.Models;
using PlcCreatorSystem_WEB.Models.Dto;
using PlcCreatorSystem_WEB.Models.VM;
using PlcCreatorSystem_WEB.Services;
using PlcCreatorSystem_WEB.Services.IServices;
using System.Security.Claims;

namespace PlcCreatorSystem_WEB.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        //public AuthController(IAuthService authService)
        //{
        //    _authService = authService;
        //}

        public AuthController(IAuthService authService, IUserService userService, IMapper mapper)
        {
            _authService = authService;
            _userService = userService;
            _mapper = mapper;
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
            if (response != null && response.IsSuccess)
            {
                LoginResponseDTO loginResponseDTO = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, loginResponseDTO.User.Id.ToString()));//added
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


        //--------------------------From UserService--------------------------

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> IndexUser()
        {
            List<UserDTO> list = new();
            var response = await _userService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<UserDTO>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUser(int userId)
        {
            UserUpdateVM userUpdateVM = new ();
            var response = await _userService.GetAsync<APIResponse>(userId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                UserDTO? model = JsonConvert.DeserializeObject<UserDTO>(Convert.ToString(response.Result));
                userUpdateVM.userVM = _mapper.Map<UserUpdateDTO>(model);
                //return View(_mapper.Map<UserUpdateDTO>(model));
                await PopulateLookups(userUpdateVM);
                return View(userUpdateVM);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UserUpdateVM model)
        {

            if (ModelState.IsValid)
            {
                TempData["success"] = "User updated successfully";
                var response = await _userService.UpdateAsync<APIResponse>(model.userVM, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "User updated successfully";
                    return RedirectToAction(nameof(IndexUser));
                }
                TempData["error"] = "Error encountered.";
            }
            await PopulateLookups(model);
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var response = await _userService.GetAsync<APIResponse>(userId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                UserDTO model = JsonConvert.DeserializeObject<UserDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(UserDTO model)
        {
            var response = await _userService.DeleteAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "User deleted successfully";
                return RedirectToAction(nameof(IndexUser));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }

        // Methods
        private async Task PopulateLookups(UserUpdateVM model)
        {
            model.RoleOptions = Enum.GetNames(typeof(SD.Role))
                .Select(i => new SelectListItem
                {
                    Value = i,
                    Text = i,
                    Selected = string.Equals(i, model.userVM.Role, StringComparison.OrdinalIgnoreCase)
                });
        }
    }
}
