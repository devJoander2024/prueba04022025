using Microsoft.AspNetCore.Mvc;
using WebApplication2.Dtos;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly CountriesServices _countries;

        public CountriesController(CountriesServices countries)
        {
            _countries = countries;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var countriesList = await _countries.GetAllCountries();
                return Ok(countriesList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("getByCode")]
        public async Task<Countries> GetByCode(string isoCode)
        {
            Countries countriesList = await _countries.GetCountryByCode(isoCode);
            return countriesList;
 
        }
    }
}
