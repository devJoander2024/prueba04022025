using Microsoft.AspNetCore.Mvc;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/calculator")]
    public class CalculadoraController : ControllerBase
    {
        public readonly CalculadoraService _calculadoraService;
        public CalculadoraController(CalculadoraService calculadoraService) {

            _calculadoraService = calculadoraService;

        }

        [HttpPost]
        public async Task<int> sumar([FromQuery] int num1, [FromQuery] int num2)
        {
            return await _calculadoraService.sumar(num1, num2);
        }
    }
}
