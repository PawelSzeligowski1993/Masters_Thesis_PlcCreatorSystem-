using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PlcCreatorSystem_WEB.Models;
using PlcCreatorSystem_Utility;
using PlcCreatorSystem_WEB.Models.Dto;
using PlcCreatorSystem_WEB.Models.VM;
using PlcCreatorSystem_WEB.Services.IServices;
using PlcCreatorSystem_WEB.Services;


namespace PlcCreatorSystem_WEB.Controllers
{
    public class HmiController : Controller
    {
        private readonly IHmiService _hmiService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public HmiController(IHmiService hmiService, IUserService userService, IMapper mapper )
        {
            _hmiService = hmiService;
            _userService = userService;
            _mapper = mapper;
        }

        [Authorize(Roles = "admin,engineer,custom")]
        public async Task<IActionResult> IndexHmi()
        {
            List<HmiDTO> list = new();
            var response = await _hmiService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<HmiDTO>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        [Authorize(Roles = "admin,engineer")]
        public async Task<IActionResult> CreateHmi()
        {
            HmiCreateVM hmiCreateVM = new();
            await PopulateLookups(hmiCreateVM);
            return View(hmiCreateVM);
        }

        [Authorize(Roles = "admin,engineer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHmi(HmiCreateVM model)
        {

            if (ModelState.IsValid)
            {
                var response = await _hmiService.CreateAsync<APIResponse>(model.hmicreateVM, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {

                    return RedirectToAction(nameof(IndexHmi));
                }
                else
                {
                    if (response.ErrorsMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorsMessages.FirstOrDefault());
                    }
                }
            }
            await PopulateLookups(model);
            return View(model);
        }

        [Authorize(Roles = "admin,engineer")]
        public async Task<IActionResult> UpdateHmi(int hmiId)
        {
            HmiUpdateVM hmiVM = new();
            var response = await _hmiService.GetAsync<APIResponse>(hmiId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                HmiDTO? model = JsonConvert.DeserializeObject<HmiDTO>(Convert.ToString(response.Result));
                hmiVM.hmiUpdateVM = _mapper.Map<HmiUpdateDTO>(model);
                //return View(_mapper.Map<HmiUpdateDTO>(model));
                await PopulateLookups(hmiVM);
                return View(hmiVM);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin,engineer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateHmi(HmiUpdateVM model)
        {

            if (ModelState.IsValid)
            {
                var response = await _hmiService.UpdateAsync<APIResponse>(model.hmiUpdateVM, HttpContext.Session.GetString(SD.SessionToken));
                TempData["success"] = "Hmi updated successfully";
                //var response = await _hmiService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexHmi));
                }
                else
                {
                    var err = 
                        response?.ErrorsMessages?.FirstOrDefault()
                        ?? (response?.StatusCode == System.Net.HttpStatusCode.BadRequest
                            ? "Bad request while updating HMI."
                            : "Unexpected error while updating HMI.");

                    ModelState.AddModelError(string.Empty, err);
                }
            }
            await PopulateLookups(model);
            return View(model);
        }

        [Authorize(Roles = "admin,engineer")]
        public async Task<IActionResult> DeleteHmi(int hmiId)
        {
            HmiDeleteVM hmitVM = new();
            var response = await _hmiService.GetAsync<APIResponse>(hmiId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                HmiDTO model = JsonConvert.DeserializeObject<HmiDTO>(Convert.ToString(response.Result));
                hmitVM.hmiDeleteVM = model;

                await PopulateLookups(hmitVM);

                return View(hmitVM);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin,engineer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHmi(HmiDeleteVM model)
        {
            var response = await _hmiService.DeleteAsync<APIResponse>(model.hmiDeleteVM.Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "HMI deleted successfully";
                return RedirectToAction(nameof(IndexHmi));
            }
            await PopulateLookups(model);
            return View(model);
        }

        //Methods
        //HmiCreateVM
        private async Task PopulateLookups(HmiCreateVM model)
        {
            var responseHmi = await _userService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (responseHmi != null && responseHmi.IsSuccess == true)
            {
                model.usersList = JsonConvert.DeserializeObject<List<UserDTO>>
                    (Convert.ToString(responseHmi.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

        }

        //HmiUpdateVM
        private async Task PopulateLookups(HmiUpdateVM model)
        {
            var responseHmi = await _userService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (responseHmi != null && responseHmi.IsSuccess == true)
            {
                model.usersList = JsonConvert.DeserializeObject<List<UserDTO>>
                    (Convert.ToString(responseHmi.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

        }

        //HmiDeleteVM
        private async Task PopulateLookups(HmiDeleteVM model)
        {
            var responseHmi = await _userService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (responseHmi != null && responseHmi.IsSuccess == true)
            {
                model.usersList = JsonConvert.DeserializeObject<List<UserDTO>>
                    (Convert.ToString(responseHmi.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

        }
    }
}
