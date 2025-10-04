using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        public async Task<IActionResult> IndexHmi()
        {
            List<HmiDTO> list = new();
            var response = await _hmiService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<HmiDTO>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        [Authorize(Roles="addmin")]
        public async Task<IActionResult> CreateHmi()
        {
            return View();
        }

        [Authorize(Roles = "addmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHmi(HmiCreateDTO model)
        {

            if (ModelState.IsValid)
            {
                var response = await _hmiService.CreateAsync<APIResponse>(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "HMI created successfully";
                    return RedirectToAction(nameof(IndexHmi));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }

        [Authorize(Roles = "addmin")]
        public async Task<IActionResult> UpdateHmi(int hmiId)
        {
            var response = await _hmiService.GetAsync<APIResponse>(hmiId);
            if (response != null && response.IsSuccess)
            {
                HmiDTO model = JsonConvert.DeserializeObject<HmiDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<HmiUpdateDTO>(model));
            }
            return NotFound();
        }

        [Authorize(Roles = "addmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateHmi(HmiUpdateDTO model)
        {

            if (ModelState.IsValid)
            {
                TempData["success"] = "Hmi updated successfully";
                var response = await _hmiService.UpdateAsync<APIResponse>(model);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexHmi));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }

        [Authorize(Roles = "addmin")]
        public async Task<IActionResult> DeleteHmi(int hmiId)
        {
            var response = await _hmiService.GetAsync<APIResponse>(hmiId);
            if (response != null && response.IsSuccess)
            {
                HmiDTO model = JsonConvert.DeserializeObject<HmiDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }

        [Authorize(Roles = "addmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHmi(HmiDTO model)
        {
            var response = await _hmiService.DeleteAsync<APIResponse>(model.Id);
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
