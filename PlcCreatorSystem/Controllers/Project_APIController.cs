using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PlcCreatorSystem.Generator;
using PlcCreatorSystem_API.Models;
using PlcCreatorSystem_API.Models.Dto;
using PlcCreatorSystem_API.Repository.IRepository;
using PlcCreatorSystem_API.TIA;
using System.Net;
using System.Net.Mime;

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
        private readonly IUserRepository _dbUSER;
        private readonly GeneratorService.GeneratorServiceClient _generator;

        public Project_APIController(IProjectRepository dbProject, IMapper mapper, IPLCRepository dbPLC, IHMIRepository dbHMI, IUserRepository dbUSER, GeneratorService.GeneratorServiceClient generator)
        {
            _dbProject = dbProject;
            _mapper = mapper;
            this._response = new();
            _dbPLC = dbPLC;
            _dbHMI = dbHMI;
            _dbUSER = dbUSER;
            _generator = generator;
        }

        [Authorize(Roles = "admin,engineer,custom")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetProjects()
        {
            try
            {
                IEnumerable<Project> projectList = await _dbProject.GetAllAsync(includeProperties: "PLC,HMI,LocalUser");
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

        [Authorize(Roles = "admin,engineer")]
        [HttpGet("{id:int}", Name = "GetProject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
                var project = await _dbProject.GetAsync(x => x.Id == id, includeProperties: "PLC,HMI,LocalUser");
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

        [Authorize(Roles = "admin,engineer")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //gRPC
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(100_000_000)]
        [RequestFormLimits(MultipartBodyLengthLimit = 100_000_000)]
        public async Task<ActionResult<APIResponse>> CreateProject([FromBody] ProjectCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorsMessages = new List<string> { "Payload cannot be null." };
                    return BadRequest(_response);
                }
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

                if (await _dbUSER.GetAsync(u => u.Id == createDTO.UserID) == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorsMessages = new List<string> { "USER ID is invalid!" };
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

        [Authorize(Roles = "admin,engineer")]
        [HttpDelete("{id:int}", Name = "DeleteProject")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] 
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

        [Authorize(Roles = "admin,engineer")]
        [HttpPut("{id:int}", Name = "UpdateProject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateProject(int id, [FromBody] ProjectUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }
                if (await _dbPLC.GetAsync(u => u.Id == updateDTO.PlcID) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "PLC ID is Invalid!");
                    return BadRequest(ModelState);
                }
                if (await _dbHMI.GetAsync(u => u.Id == updateDTO.HmiID) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "HMI ID is Invalid!");
                    return BadRequest(ModelState);
                }
                if (await _dbUSER.GetAsync(u => u.Id == updateDTO.UserID) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "User ID is Invalid!");
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
        // ----------------------- gRPC END POINTS-----------------------------------
        [Authorize(Roles = "admin,engineer")]
        [HttpPost("{id:int}/csv")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(100_000_000)]
        [RequestFormLimits(MultipartBodyLengthLimit = 100_000_000)]
        public async Task<IActionResult> UploadCsv(int id, IFormFile csvFile)
        {
            if (csvFile is null || csvFile.Length == 0)
                return BadRequest(new APIResponse { IsSuccess = false, ErrorsMessages = { "CSV file is required." } });

            var project = await _dbProject.GetAsync(p => p.Id == id);
            if (project is null)
                return NotFound(new APIResponse { IsSuccess = false, ErrorsMessages = { "Project not found." } });

            const int chunk = 64 * 1024;
            var buffer = new byte[chunk];

            using var call = _generator.UploadCsvStream();
            await using var s = csvFile.OpenReadStream();

            int read; bool first = true;
            while ((read = await s.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                var up = new UploadChunk { Data = Google.Protobuf.ByteString.CopyFrom(buffer, 0, read) };
                if (first) { up.FileName = csvFile.FileName; first = false; }
                await call.RequestStream.WriteAsync(up);
            }
            await call.RequestStream.CompleteAsync();
            var reply = await call.ResponseAsync;
            if (!reply.Ok) return Problem($"CSV upload failed: {reply.Msg}");

            return Ok(new APIResponse { IsSuccess = true, Result = new { id, msg = "CSV uploaded" } });
        }

        [Authorize(Roles = "admin,engineer")]
        [HttpGet("{id:int}/download")]
        public async Task<IActionResult> DownloadTiaProject(int id, [FromServices] IOptions<TiaStorageOptions> tia)
        {
            var project = await _dbProject.GetAsync(p => p.Id == id);
            if (project == null)
                return NotFound(new APIResponse { IsSuccess = false, ErrorsMessages = { "Project not found." } });

            var name = string.IsNullOrWhiteSpace(project.Name) ? "Project" : project.Name;
            var path = Path.Combine(tia.Value.TiaRoot, $"{name}.zap17");

            if (!System.IO.File.Exists(path))
                return NotFound(new APIResponse { IsSuccess = false, ErrorsMessages = { $"TIA project not ready: {name}.zap17" } });

            var stream = System.IO.File.OpenRead(path);
            return File(stream, MediaTypeNames.Application.Octet, $"{name}.zap17");
        }


    }
}
