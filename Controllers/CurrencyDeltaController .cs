using ExchangeDelta.Api.Services;
using ExchangeDelta.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeDelta.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyDeltaController : ControllerBase
    {
        private readonly CurrencyDeltaService _currencyDeltaService;

        public CurrencyDeltaController(CurrencyDeltaService currencyDeltaService)
        {
            _currencyDeltaService = currencyDeltaService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CurrencyDeltaRequest request)
        {
            var (result, error) = await _currencyDeltaService.GetDeltasAsync(request);

            if (error != null)
                return BadRequest(error);

            return Ok(result);
        }
    }
}