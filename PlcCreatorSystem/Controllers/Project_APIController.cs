using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlcCreatorSystem_API.Models;
using PlcCreatorSystem_API.Models.Dto;
using PlcCreatorSystem_API.Repository.IRepository;
using System.Net;

namespace PlcCreatorSystem_API.Controllers
{
    [Route("api/Project_API")]
    [ApiController]
    public class Project_APIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IProjectRepository _dbProject;
        private readonly IMapper _mapper;

        public Project_APIController(IProjectRepository dbProject, IMapper mapper)
        {
            _dbProject = dbProject;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetProjects()
        {
            try
            {
                IEnumerable<Project> ProjectList = await _dbProject.GetAllAsync();
                _response.Result = _mapper.Map<List<ProjectDTO>>(ProjectList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;

        }
    }
}
