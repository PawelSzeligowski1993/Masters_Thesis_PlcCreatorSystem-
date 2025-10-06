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
    public class PlcController : Controller
    {
        private readonly IPlcService _plcService;
        private readonly IMapper _mapper;

        public PlcController(IPlcService plcService, IMapper mapper)
        {
            _plcService = plcService;
            _mapper = mapper;
        }

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

        [Authorize(Roles = "admin,enginner")]
        public async Task<IActionResult> CreatePlc()
        {
            return View();
        }

        [Authorize(Roles = "admin,enginner")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePlc(PlcCreateDTO model)
        {

            if (ModelState.IsValid)
            {
                var response = await _plcService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Plc created successfully";
                    return RedirectToAction(nameof(IndexPlc));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }

        [Authorize(Roles = "admin,enginner")]
        public async Task<IActionResult> UpdatePlc(int plcId)
        {
            var response = await _plcService.GetAsync<APIResponse>(plcId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                PlcDTO model = JsonConvert.DeserializeObject<PlcDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<PlcUpdateDTO>(model));
            }
            return NotFound();
        }

        [Authorize(Roles = "admin,enginner")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePlc(PlcUpdateDTO model)
        {

            if (ModelState.IsValid)
            {
                TempData["success"] = "Plc updated successfully";
                var response = await _plcService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexPlc));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }

        [Authorize(Roles = "admin,enginner")]
        public async Task<IActionResult> DeletePlc(int plcId)
        {
            var response = await _plcService.GetAsync<APIResponse>(plcId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                PlcDTO model = JsonConvert.DeserializeObject<PlcDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin,enginner")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePlc(PlcDTO model)
        {
            var response = await _plcService.DeleteAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Plc deleted successfully";
                return RedirectToAction(nameof(IndexPlc));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
    }
}
