using ContactBookApi.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }


        [HttpGet("GetAllCountry")]
        public IActionResult GetAllCountry()
        {
            var response = _countryService.GetAllCountry();
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
