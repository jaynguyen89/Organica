using System.Threading.Tasks;
using HelperLibrary;
using Hidrogen.Attributes;
using Hidrogen.Services;
using Hidrogen.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hidrogen.Controllers {
    
    [ApiController]
    [Route("country")]
    public class CountryController {
        
        private readonly ILogger<CountryController> _logger;
        private readonly ICountryService _countryService;

        public CountryController(
            ILogger<CountryController> logger,
            ICountryService countryService
        ) {
            _logger = logger;
            _countryService = countryService;
        }
        
        [HttpGet("compact-countries")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,1,0,0,0,0,0,0")]
        public async Task<JsonResult> GetCountriesForDropdown() {
            _logger.LogInformation("CountryController.GetCountriesForDropdown - Service starts.");

            var result = await _countryService.GetCompactCountries();
            
            return new JsonResult(new { Result = HidroEnums.RESULTS.SUCCESS, Message = result });
        }
    }
}