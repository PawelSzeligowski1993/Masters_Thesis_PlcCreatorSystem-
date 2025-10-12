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
using System.Security.Claims;
using System.Net.Http.Headers;

namespace PlcCreatorSystem_WEB.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IPlcService _plcService;
        private readonly IHmiService _hmiService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        

        public ProjectController(IProjectService projectService, IPlcService plcService, IHmiService hmiService, IUserService userService, IMapper mapper, IConfiguration configuration)
        {
            _projectService = projectService;
            _plcService = plcService;
            _hmiService = hmiService;
            _userService = userService;
            _mapper = mapper;
            _configuration = configuration;
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
            projectVM.projectCreateVM.Status = SD.ProjectStatus.waiting_to_check;
            return View(projectVM);
        }

        [Authorize(Roles = "admin,engineer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProject(ProjectCreateVM model)
        {
            //default status = "waiting_to_check"
            ModelState.Remove("projectCreateVM.Status");
            model.projectCreateVM.Status = SD.ProjectStatus.waiting_to_check;

            if (ModelState.IsValid)
            {
                var response = await _projectService.CreateAsync<APIResponse>(model.projectCreateVM, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Project created successfully";
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
                projectVM.projectUpdateVM = _mapper.Map<ProjectUpdateDTO>(model);
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
                var response = await _projectService.UpdateAsync<APIResponse>(model.projectUpdateVM, HttpContext.Session.GetString(SD.SessionToken));
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
                TempData["success"] = "Project deleted successfully";
                return RedirectToAction(nameof(IndexProject));
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

        //---------------------------------------------------------------------------------------------
        //************************************ gRPC Controlers ****************************************
        //---------------------------------------------------------------------------------------------

        //-------------------------------- gRPC POST and Get CSV Files --------------------------------
        // GET: /UploadCsv/UploadCsv
        [Authorize(Roles = "admin,engineer")]
        [HttpGet]
        public IActionResult UploadCsv()
        {
            return View(new UploadCsvVM());  
        }

        // POST: /UploadCsv/UploadCsv
        [Authorize(Roles = "admin,engineer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadCsv(UploadCsvVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var token = HttpContext.Session.GetString(SD.SessionToken) ?? string.Empty;

            var resp = await _projectService.UploadCsvAsync(vm.ProjectId, vm.CsvFile, token);

            if (resp.IsSuccess)
            {
                TempData["success"] = "CSV uploaded successfully.";
                return RedirectToAction("IndexProject", "Project");
            }

            // show API error, if any
            var msg = resp.ErrorsMessages?.FirstOrDefault() ?? "Upload failed.";
            ModelState.AddModelError("ErrorMessages", msg);
            return View(vm);
        }



        //-------------------------------- gRPC Downland TIA.zap File --------------------------------
        [Authorize(Roles = "admin,engineer,custom")]
        public async Task<IActionResult> Download(int id)
        {
            var token = HttpContext.Session.GetString(PlcCreatorSystem_Utility.SD.SessionToken) ?? "";
            var apiBase = _configuration["ServiceUrls:Creator_API"]; // e.g. https://localhost:7251
            var url = $"{apiBase}/api/Project_API/{id}/download";

            using var client = new HttpClient();
            if (!string.IsNullOrWhiteSpace(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var resp = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            if (!resp.IsSuccessStatusCode)
            {
                TempData["error"] = "TIA project not available yet.";
                return RedirectToAction(nameof(IndexProject));
            }

            var fileName = resp.Content.Headers.ContentDisposition?.FileName?.Trim('\"')
                           ?? $"Project_{id}.zap17";
            var stream = await resp.Content.ReadAsStreamAsync();

            return File(stream, "application/octet-stream", fileName);
        }

        //************************************************************************************************
        //****************************** Methods - PopulateLookups ***************************************
        //************************************************************************************************
        // --------------------------- Populate View with PLC, HMI, and USER -----------------------------
        //ProjectCreateVM
        private async Task PopulateLookups(ProjectCreateVM model)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr, out var id))
            {
                model.projectCreateVM.UserID = id;
            }
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

            var responseUser = await _userService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (responseUser != null && responseUser.IsSuccess == true)
            {
                model.usersList = JsonConvert.DeserializeObject<List<UserDTO>>
                    (Convert.ToString(responseUser.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
        }

        //ProjectUpdateVM
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

            var responseUser = await _userService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (responseUser != null && responseUser.IsSuccess == true)
            {
                model.usersList = JsonConvert.DeserializeObject<List<UserDTO>>
                    (Convert.ToString(responseUser.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
        }

        //ProjectDeleteVM           
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

            model.statusList = Enum.GetValues(typeof(SD.ProjectStatus)).Cast<SD.ProjectStatus>()
                .Select(i => new SelectListItem
                {
                Text = i.ToString(),
                Value = i.ToString()
                });

            var responseUser = await _userService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (responseUser != null && responseUser.IsSuccess == true)
            {
                model.usersList = JsonConvert.DeserializeObject<List<UserDTO>>
                    (Convert.ToString(responseUser.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
        }
    }
}
