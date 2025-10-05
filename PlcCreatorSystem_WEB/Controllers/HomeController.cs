using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlcCreatorSystem_Utility;
using PlcCreatorSystem_WEB.Models;
using PlcCreatorSystem_WEB.Models.Dto;
using PlcCreatorSystem_WEB.Services;
using PlcCreatorSystem_WEB.Services.IServices;

namespace PlcCreatorSystem_WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public HomeController(IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<ProjectDTO> list = new();
            var response = await _projectService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProjectDTO>>(Convert.ToString(response.Result));
            }

            return View(list);
        }
    }
}
