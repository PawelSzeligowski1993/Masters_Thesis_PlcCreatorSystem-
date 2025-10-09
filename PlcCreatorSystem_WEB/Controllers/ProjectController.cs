using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PlcCreatorSystem_Utility;
using PlcCreatorSystem_WEB.Models;
using PlcCreatorSystem_WEB.Models.Dto;
using PlcCreatorSystem_WEB.Models.VM;
using PlcCreatorSystem_WEB.Services.IServices;
using PlcCreatorSystem_Utility;

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

        [Authorize(Roles = "admin,engineer,custom")]
        public async Task<IActionResult> IndexProject()
        {
            List<ProjectDTO>? list = new();
            var response = await _projectService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProjectDTO>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        [Authorize(Roles = "admin,engineer")]
        public async Task<IActionResult> CreateProject()
        {
            ProjectCreateVM projectVM = new();
            await PopulateLookups(projectVM);
            //default status = "waiting_to_check"
            projectVM.projectVM.Status = SD.ProjectStatus.waiting_to_check;
            return View(projectVM);
        }

        [Authorize(Roles = "admin,engineer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProject(ProjectCreateVM model)
        {
            //default status = "waiting_to_check"
            ModelState.Remove("project.Status");
            model.projectVM.Status = SD.ProjectStatus.waiting_to_check;

            if (ModelState.IsValid)
            {
                var response = await _projectService.CreateAsync<APIResponse>(model.projectVM, HttpContext.Session.GetString(SD.SessionToken));
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

        [Authorize(Roles = "admin,engineer")]
        public async Task<IActionResult> UpdateProject(int id)
        {
            ProjectUpdateVM projectVM = new();
            var response = await _projectService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                ProjectDTO? model = JsonConvert.DeserializeObject<ProjectDTO>(Convert.ToString(response.Result));
                projectVM.projectVM = _mapper.Map<ProjectUpdateDTO>(model);
                //return View(_mapper.Map<ProjectUpdateDTO>(model));

                await PopulateLookups(projectVM);
                return View(projectVM);
            }

            return NotFound();
        }

        [Authorize(Roles = "admin,engineer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProject(ProjectUpdateVM model)
        {

            if (ModelState.IsValid)
            {
                var response = await _projectService.UpdateAsync<APIResponse>(model.projectVM, HttpContext.Session.GetString(SD.SessionToken));
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

        [Authorize(Roles = "admin,engineer")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            ProjectDeleteVM projectVM = new();
            var response = await _projectService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                ProjectDTO? model = JsonConvert.DeserializeObject<ProjectDTO>(Convert.ToString(response.Result));
                projectVM.projectVM = model;

                await PopulateLookups(projectVM);
                return View(projectVM);
            }

            return NotFound();
        }

        [Authorize(Roles = "admin,engineer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProject(ProjectDeleteVM model)
        {
            var response = await _projectService.DeleteAsync<APIResponse>(model.projectVM.Id, HttpContext.Session.GetString(SD.SessionToken));
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
            var responsePlc = await _plcService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (responsePlc != null && responsePlc.IsSuccess == true)
            {
                model.plcList = JsonConvert.DeserializeObject<List<PlcDTO>>
                    (Convert.ToString(responsePlc.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

            var responseHmi = await _hmiService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (responseHmi != null && responseHmi.IsSuccess)
            {
                model.hmiList = JsonConvert.DeserializeObject<List<HmiDTO>>
                    (Convert.ToString(responseHmi.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

            model.statusList = Enum.GetValues(typeof(SD.ProjectStatus)).Cast<SD.ProjectStatus>()
                .Select(i => new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = (i == SD.ProjectStatus.waiting_to_check)
                });
        }

        private async Task PopulateLookups(ProjectUpdateVM model)
        {
            var responsePlc = await _plcService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (responsePlc != null && responsePlc.IsSuccess)
            {
                model.plcList = JsonConvert.DeserializeObject<List<PlcDTO>>
                    (Convert.ToString(responsePlc.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

            var responseHmi = await _hmiService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (responseHmi != null && responseHmi.IsSuccess)
            {
                model.hmiList = JsonConvert.DeserializeObject<List<HmiDTO>>
                    (Convert.ToString(responseHmi.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

            model.statusList = Enum.GetValues(typeof(SD.ProjectStatus)).Cast<SD.ProjectStatus>()
                .Select(i => new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
        }

        private async Task PopulateLookups(ProjectDeleteVM model)
        {
            var responsePlc = await _plcService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (responsePlc != null && responsePlc.IsSuccess)
            {
                model.plcList = JsonConvert.DeserializeObject<List<PlcDTO>>
                    (Convert.ToString(responsePlc.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

            var responseHmi = await _hmiService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
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
