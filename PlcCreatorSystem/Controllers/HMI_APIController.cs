using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PlcCreatorSystem_API.Models;
using PlcCreatorSystem_API.Models.Dto;
using PlcCreatorSystem_API.Repository.IRepository;
using System.Net;

namespace PlcCreatorSystem_API.Controllers
{
    [Route("api/HMI_API")]
    [ApiController]
    public class HMI_APIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IHMIRepository _dbHMI;
        private readonly IMapper _mapper;

        public HMI_APIController(IHMIRepository dbHMI, IMapper mapper)
        {
            _dbHMI = dbHMI;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetHMI()
        {
            try
            {
                IEnumerable<HMI> hmiList = await _dbHMI.GetAllAsync();
                _response.Result = _mapper.Map<List<HmiDTO>>(hmiList);
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

        [Authorize(Roles ="admin")]
        [HttpGet("{id:int}", Name = "GetHMI")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(HmiDTO), 200)]
        public async Task<ActionResult<APIResponse>> GetHMI(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var project = await _dbHMI.GetAsync(x => x.Id == id);
                if (project == null)
                {
                    return NotFound();
                }
                _response.Result = _mapper.Map<HmiDTO>(project);
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

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<APIResponse>> CreateHMI([FromBody] HmiCreateDTO createDTO)
        {
            try
            {
                if (await _dbHMI.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "HMI already Exist!");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                HMI hmi = _mapper.Map<HMI>(createDTO);

                await _dbHMI.CreateAsync(hmi);
                _response.Result = _mapper.Map<HmiDTO>(hmi);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetHMI", new { id = hmi.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}", Name = "DeleteHMI")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteHMI(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var hmi = await _dbHMI.GetAsync(x => x.Id == id);
                if (hmi == null)
                {
                    return NotFound();
                }
                await _dbHMI.RemoveAsync(hmi);
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

        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}", Name = "UpdateHMI")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateHMI(int id, [FromBody] HmiDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

                HMI model = _mapper.Map<HMI>(updateDTO);

                await _dbHMI.UpdateAsync(model);
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

        [Authorize(Roles = "admin")]
        [HttpPatch("{id:int}", Name = "UpdatePartialHMI")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialHMI(int id, JsonPatchDocument<HmiUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            { return BadRequest(); }

            var hmi = await _dbHMI.GetAsync(u => u.Id == id, tracked: false);

            HmiUpdateDTO hmiDTO = _mapper.Map<HmiUpdateDTO>(hmi);


            if (hmi == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(hmiDTO, ModelState);
            HMI model = _mapper.Map<HMI>(hmiDTO);


            await _dbHMI.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
