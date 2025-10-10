using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PlcCreatorSystem_Utility;
using PlcCreatorSystem_WEB.Models;
using PlcCreatorSystem_WEB.Models.Dto;
using PlcCreatorSystem_WEB.Models.VM;
using PlcCreatorSystem_WEB.Services.IServices;
using System.Security.Claims;

namespace PlcCreatorSystem_WEB.Controllers
{
    public class PlcController : Controller
    {
        private readonly IPlcService _plcService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public PlcController(IPlcService plcService, IUserService userService, IMapper mapper)
        {
            _plcService = plcService;
            _userService = userService;
            _mapper = mapper;
        }

        [Authorize(Roles = "admin,engineer,custom")]
        public async Task<IActionResult> IndexPlc()
        {
            List<PlcDTO> list = new();
            var response = await _plcService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<PlcDTO>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        [Authorize(Roles = "admin,engineer")]
        public async Task<IActionResult> CreatePlc()
        {
            PlcCreateVM plcCreateVM = new();
            await PopulateLookups(plcCreateVM);
            return View(plcCreateVM);
        }

        [Authorize(Roles = "admin,engineer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePlc(PlcCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _plcService.CreateAsync<APIResponse>(model.plcCreateVM, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Plc created successfully";
                    return RedirectToAction(nameof(IndexPlc));
                }
                else
                {
                    if (response.ErrorsMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorsMessages.FirstOrDefault());
                    }
                }
            }else
            {
                //ModelState.AddModelError("ErrorMessages", model.ErrorsMessages.FirstOrDefault());//Error model is not Valid
            }
            await PopulateLookups(model);
            return View(model);
        }

        [Authorize(Roles = "admin,engineer")]
        public async Task<IActionResult> UpdatePlc(int plcId)
        {
            PlcUpdateVM plcVM = new();
            var response = await _plcService.GetAsync<APIResponse>(plcId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                PlcDTO model = JsonConvert.DeserializeObject<PlcDTO>(Convert.ToString(response.Result));
                plcVM.plcUpdateVM = _mapper.Map<PlcUpdateDTO>(model);
                await PopulateLookups(plcVM);
                return View(plcVM);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin,engineer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePlc(PlcUpdateVM model)
        {

            if (ModelState.IsValid)
            {
                TempData["success"] = "Plc updated successfully";
                var response = await _plcService.UpdateAsync<APIResponse>(model.plcUpdateVM, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexPlc));
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
        public async Task<IActionResult> DeletePlc(int plcId)
        {
            PlcDeleteVM plcVM = new();
            var response = await _plcService.GetAsync<APIResponse>(plcId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                PlcDTO model = JsonConvert.DeserializeObject<PlcDTO>(Convert.ToString(response.Result));
                plcVM.plcDeleteVM = model;

                await PopulateLookups(plcVM);

                return View(plcVM);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin,engineer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePlc(PlcDeleteVM model)
        {
            var response = await _plcService.DeleteAsync<APIResponse>(model.plcDeleteVM.Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "PLC deleted successfully";
                return RedirectToAction(nameof(IndexPlc));
            }
            else
            {
                if (response.ErrorsMessages.Count > 0)
                {
                    ModelState.AddModelError("ErrorMessages", response.ErrorsMessages.FirstOrDefault());
                }
            }
            await PopulateLookups(model);
            return View(model);
        }
        //************************************************************************************************
        //****************************** Methods - PopulateLookups ***************************************
        //************************************************************************************************
        //HmiCreateVM
        private async Task PopulateLookups(PlcCreateVM model)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr, out var id))
            {
                model.plcCreateVM.UserID = id;
            }
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
        private async Task PopulateLookups(PlcUpdateVM model)
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
        private async Task PopulateLookups(PlcDeleteVM model)
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
