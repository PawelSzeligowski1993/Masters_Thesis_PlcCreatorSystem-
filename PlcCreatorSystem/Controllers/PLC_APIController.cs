using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PlcCreatorSystem_API.Models.Dto;
using PlcCreatorSystem_API.Models;
using PlcCreatorSystem_API.Repository.IRepository;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PlcCreatorSystem_API.Controllers
{
    [Route("api/PLC_API")]
    [ApiController]
    public class PLC_APIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IPLCRepository _dbPLC;
        private readonly IMapper _mapper;
        private readonly IUserRepository _dbUSER;

        public PLC_APIController(IPLCRepository dbPLC, IMapper mapper, IUserRepository dbUSER)
        {
            _dbPLC = dbPLC;
            _mapper = mapper;
            this._response = new();
            _dbUSER = dbUSER;
        }

        [Authorize(Roles = "admin,engineer,custom")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetPLC()
        {
            try
            {
                IEnumerable<PLC> pLCList = await _dbPLC.GetAllAsync(includeProperties: "LocalUser");
                _response.Result = _mapper.Map<List<PlcDTO>>(pLCList);
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
        [HttpGet("{id:int}", Name = "GetPLC")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(PlcDTO), 200)]
        public async Task<ActionResult<APIResponse>> GetPLC(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var plc = await _dbPLC.GetAsync(x => x.Id == id, includeProperties: "LocalUser");
                if (plc == null)
                {
                    return NotFound();
                }
                _response.Result = _mapper.Map<PlcDTO>(plc);
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
        public async Task<ActionResult<APIResponse>> CreatePLC([FromBody] PlcCreateDTO createDTO)
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
                if (await _dbPLC.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    ModelState.AddModelError("ErrorMessages", "Plc already Exist!");
                    return BadRequest(ModelState);
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

                PLC plc = _mapper.Map<PLC>(createDTO);

                await _dbPLC.CreateAsync(plc);
                _response.Result = _mapper.Map<PlcDTO>(plc);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetPlc", new { id = plc.Id }, _response);
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
        [HttpDelete("{id:int}", Name = "DeletePLC")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeletePLC(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var plc = await _dbPLC.GetAsync(x => x.Id == id);
                if (plc == null)
                {
                    return NotFound();
                }
                await _dbPLC.RemoveAsync(plc);
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
        [HttpPut("{id:int}", Name = "UpdatePlc")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdatePLC(int id, [FromBody] PlcUpdateDTO updateDTO)
        {
            try
            {
                if (await _dbUSER.GetAsync(u => u.Id == updateDTO.UserID) == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorsMessages = new List<string> { "USER ID is invalid!" };
                    return BadRequest(_response);
                }
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

                if (await _dbPLC.GetAsync(u => u.Id == updateDTO.UserID) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "User ID is Invalid!");
                    return BadRequest(ModelState);
                }

                PLC model = _mapper.Map<PLC>(updateDTO);

                await _dbPLC.UpdateAsync(model);
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
    }
}
