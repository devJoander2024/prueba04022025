
using Microsoft.AspNetCore.Mvc;
using ServiceReference1;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
        [ApiController]
        [Route("api/países")]
        public class CountryController : ControllerBase
        {
            private readonly CountryServices _countryService;

            public CountryController(CountryServices countryServices)
            {
                _countryService = countryServices;
            }

            [HttpGet]
            public async Task<IActionResult> GetCountries()
            {
            ListOfCountryNamesByCodeResponse list = await _countryService.GetCountriesAsync();
            Console.WriteLine("list.Body");
            Console.WriteLine(list.Body.ListOfCountryNamesByCodeResult);
            return Ok(list.Body.ListOfCountryNamesByCodeResult);
        }
    }
}
