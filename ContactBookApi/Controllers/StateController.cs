using ContactBookApi.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IStateService _stateService;
        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }


        [HttpGet("GetAllState")]
        public IActionResult GetAllState()
        {
            var response = _stateService.GetAllState();
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("GetStateByCountryId/{id}")]

        public IActionResult GetStateByCountryId(int id)
        {
            if (id > 0)
            {
                var result = _stateService.GetStateByCountryId(id);
                return !result.Success ? BadRequest(result) : Ok(result);
            }
            else
            {
                return BadRequest("Please enter proper data");
            }
        }

    }
}
