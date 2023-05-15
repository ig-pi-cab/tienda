using Infraestructure.ExternalApiService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DolarController : BaseApiController
    {
        private readonly IExternalApiService _externalApiService;

        public DolarController(IExternalApiService externalApiService)
        {
            _externalApiService = externalApiService;
        }

        [HttpGet]

        public async Task<ActionResult<decimal>> GetDollarExchangeRate()
        {
            try
            {
                decimal exchangeRate = await _externalApiService.GetDollarExchangeRateAsync();
                return Ok(exchangeRate);
            }
            catch (Exception ex)
            {
                // Handle the exception and return an appropriate error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve the dollar exchange rate.");
            }
        }
    }
}
