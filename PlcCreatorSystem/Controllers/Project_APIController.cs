using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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
        private readonly IPLCRepository _dbPLC;
        private readonly IHMIRepository _dbHMI;
        private readonly IMapper _mapper;

        public Project_APIController(IProjectRepository dbProject, IMapper mapper, IPLCRepository dbPLC, IHMIRepository dbHMI)
        {
            _dbProject = dbProject;
            _mapper = mapper;
            this._response = new();
            _dbPLC = dbPLC;
            _dbHMI = dbHMI;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetProjects()
        {
            try
            {
                IEnumerable<Project> projectList = await _dbProject.GetAllAsync(includeProperties: "PLC,HMI");
                _response.Result = _mapper.Map<List<ProjectDTO>>(projectList);
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

        [HttpGet("{id:int}", Name = "GetProject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProjectDTO), 200)]
        public async Task<ActionResult<APIResponse>> GetProject(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var project = await _dbProject.GetAsync(x => x.Id == id, includeProperties: "PLC,HMI");
                if (project == null)
                {
                    return NotFound();
                }
                _response.Result = _mapper.Map<ProjectDTO>(project);
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<APIResponse>> CreateProject([FromBody] ProjectCreateDTO createDTO)
        {
            try
            {
                if (await _dbProject.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    ModelState.AddModelError("ErrorMessages", "Project already Exist!");
                    return BadRequest(ModelState);
                }

                if (await _dbPLC.GetAsync(u => u.Id == createDTO.PlcID) == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorsMessages = new List<string> { "PLC ID is invalid!" };
                    return BadRequest(_response);
                }

                if (await _dbHMI.GetAsync(u => u.Id == createDTO.HmiID) == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorsMessages = new List<string> { "HMI ID is invalid!" };
                    return BadRequest(_response);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                Project project = _mapper.Map<Project>(createDTO);

                await _dbProject.CreateAsync(project);
                _response.Result = _mapper.Map<ProjectDTO>(project);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetProject", new { id = project.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteProject")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteProject(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var project = await _dbProject.GetAsync(x => x.Id == id);
                if (project == null)
                {
                    return NotFound();
                }
                await _dbProject.RemoveAsync(project);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
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

        [HttpPut("{id:int}", Name = "UpdateProject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateProject(int id, [FromBody] ProjectUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }
                if (await _dbProject.GetAsync(u => u.PlcID == updateDTO.PlcID) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "PLC ID is Invalid!");
                    return BadRequest(ModelState);
                }
                if (await _dbProject.GetAsync(u => u.HmiID == updateDTO.HmiID) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "HMI ID is Invalid!");
                    return BadRequest(ModelState);
                }
                Project model = _mapper.Map<Project>(updateDTO);

                await _dbProject.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
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

        //[HttpPatch("{id:int}", Name = "UpdatePartialProject")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> UpdatePartialProject(int id, JsonPatchDocument<ProjectUpdateDTO> patchDTO)
        //{
        //    if (patchDTO == null || id == 0)
        //    { return BadRequest(); }

        //    var project = await _dbProject.GetAsync(u => u.Id == id, tracked: false);

        //    ProjectUpdateDTO projectDTO = _mapper.Map<ProjectUpdateDTO>(project);


        //    if (project == null)
        //    {
        //        return BadRequest();
        //    }
        //    patchDTO.ApplyTo(projectDTO, ModelState);
        //    Project model = _mapper.Map<Project>(projectDTO);


        //    await _dbProject.UpdateAsync(model);

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    return NoContent();
        //}
    }
}
