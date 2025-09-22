using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PlcCreatorSystem_WEB.Models;
using PlcCreatorSystem_WEB.Models.Dto;
using PlcCreatorSystem_WEB.Models.VM;
using PlcCreatorSystem_WEB.Services.IServices;

namespace PlcCreatorSystem_WEB.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IPlcService _plcService;
        private readonly IHmiService _hmiService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectService projectService, IMapper mapper, IPlcService plcService, IHmiService hmiService)
        {
            _projectService = projectService;
            _mapper = mapper;
            _plcService = plcService;
            _hmiService = hmiService;
        }

        public async Task<IActionResult> IndexProject()
        {
            List<ProjectDTO>? list = new();
            var response = await _projectService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProjectDTO>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        public async Task<IActionResult> CreateProject()
        {
            ProjectCreateVM projectVM = new();
            await PopulateLookups(projectVM);
            return View(projectVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProject(ProjectCreateVM model)
        {

            if (ModelState.IsValid)
            {
                var response = await _projectService.CreateAsync<APIResponse>(model.project);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexProject));
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

        public async Task<IActionResult> UpdateProject(int id)
        {
            ProjectUpdateVM projectVM = new();
            var response = await _projectService.GetAsync<APIResponse>(id);
            if (response != null && response.IsSuccess)
            {
                ProjectDTO? model = JsonConvert.DeserializeObject<ProjectDTO>(Convert.ToString(response.Result));
                projectVM.project = _mapper.Map<ProjectUpdateDTO>(model);
            }
            await PopulateLookups(projectVM);
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProject(ProjectUpdateVM model)
        {

            if (ModelState.IsValid)
            {
                var response = await _projectService.UpdateAsync<APIResponse>(model.project);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexProject));
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

        public async Task<IActionResult> DeleteProject(int id)
        {
            ProjectDeleteVM projectVM = new();
            var response = await _projectService.GetAsync<APIResponse>(id);
            if (response != null && response.IsSuccess)
            {
                ProjectDTO? model = JsonConvert.DeserializeObject<ProjectDTO>(Convert.ToString(response.Result));
                projectVM.project = model;
            }

            await PopulateLookups(projectVM);
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProject(ProjectDeleteVM model)
        {
            var response = await _projectService.DeleteAsync<APIResponse>(model.project.Id);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexProject));
            }
            await PopulateLookups(model);
            return View(model);
        }

        /*----------------------------------------------------------------------------------------*/

        // --------------------------- Populate View with PLC and HMI -----------------------------
        private async Task PopulateLookups(ProjectCreateVM model)
        {
            var responsePlc = await _plcService.GetAllAsync<APIResponse>();
            if (responsePlc != null && responsePlc.IsSuccess == true)
            {
                model.plcList = JsonConvert.DeserializeObject<List<PlcDTO>>
                    (Convert.ToString(responsePlc.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

            var responseHmi = await _hmiService.GetAllAsync<APIResponse>();
            if (responseHmi != null && responseHmi.IsSuccess)
            {
                model.hmiList = JsonConvert.DeserializeObject<List<HmiDTO>>
                    (Convert.ToString(responseHmi.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
        }

        private async Task PopulateLookups(ProjectUpdateVM model)
        {
            var responsePlc = await _plcService.GetAllAsync<APIResponse>();
            if (responsePlc != null && responsePlc.IsSuccess)
            {
                model.plcList = JsonConvert.DeserializeObject<List<PlcDTO>>
                    (Convert.ToString(responsePlc.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

            var responseHmi = await _hmiService.GetAllAsync<APIResponse>();
            if (responseHmi != null && responseHmi.IsSuccess)
            {
                model.hmiList = JsonConvert.DeserializeObject<List<HmiDTO>>
                    (Convert.ToString(responseHmi.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
        }

        private async Task PopulateLookups(ProjectDeleteVM model)
        {
            var responsePlc = await _plcService.GetAllAsync<APIResponse>();
            if (responsePlc != null && responsePlc.IsSuccess)
            {
                model.plcList = JsonConvert.DeserializeObject<List<PlcDTO>>
                    (Convert.ToString(responsePlc.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

            var responseHmi = await _hmiService.GetAllAsync<APIResponse>();
            if (responseHmi != null && responseHmi.IsSuccess)
            {
                model.hmiList = JsonConvert.DeserializeObject<List<HmiDTO>>
                    (Convert.ToString(responseHmi.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
        }
    }
}
