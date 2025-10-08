using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlcCreatorSystem_Utility;
using PlcCreatorSystem_WEB.Models;
using PlcCreatorSystem_WEB.Models.Dto;
using PlcCreatorSystem_WEB.Services.IServices;

namespace PlcCreatorSystem_WEB.Controllers
{
    public class HmiController : Controller
    {
        private readonly IHmiService _hmiService;
        private readonly IMapper _mapper;

        public HmiController(IHmiService hmiService, IMapper mapper)
        {
            _hmiService = hmiService;
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
            return View();
        }

        [Authorize(Roles = "admin,engineer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHmi(HmiCreateDTO model)
        {

            if (ModelState.IsValid)
            {
                var response = await _hmiService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "HMI created successfully";
                    return RedirectToAction(nameof(IndexHmi));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }

        [Authorize(Roles = "admin,engineer")]
        public async Task<IActionResult> UpdateHmi(int hmiId)
        {
            var response = await _hmiService.GetAsync<APIResponse>(hmiId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                HmiDTO model = JsonConvert.DeserializeObject<HmiDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<HmiUpdateDTO>(model));
            }
            return NotFound();
        }

        [Authorize(Roles = "admin,engineer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateHmi(HmiUpdateDTO model)
        {

            if (ModelState.IsValid)
            {
                TempData["success"] = "Hmi updated successfully";
                var response = await _hmiService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexHmi));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }

        [Authorize(Roles = "admin,engineer")]
        public async Task<IActionResult> DeleteHmi(int hmiId)
        {
            var response = await _hmiService.GetAsync<APIResponse>(hmiId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                HmiDTO model = JsonConvert.DeserializeObject<HmiDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin,engineer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHmi(HmiDTO model)
        {
            var response = await _hmiService.DeleteAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "HMI deleted successfully";
                return RedirectToAction(nameof(IndexHmi));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }

    }
}
