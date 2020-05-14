using System.Threading.Tasks;
using HelperLibrary;
using Hidrogen.Attributes;
using Hidrogen.Services;
using Hidrogen.Services.Interfaces;
using MethaneLibrary.Interfaces;
using MethaneLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hidrogen.Controllers {
    
    [ApiController]
    [Route("country")]
    public class CountryController {
        
        private readonly ILogger<CountryController> _logger;
        private readonly IRuntimeLogService _runtimeLogger;
        private readonly ICountryService _countryService;

        public CountryController(
            ILogger<CountryController> logger,
            IRuntimeLogService runtimeLogger,
            ICountryService countryService
        ) {
            _logger = logger;
            _runtimeLogger = runtimeLogger;
            _countryService = countryService;
        }
        
        [HttpGet("compact-countries")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,1,0,0,0,0,0,0")]
        public async Task<JsonResult> GetCountriesForDropdown() {
            _logger.LogInformation("CountryController.GetCountriesForDropdown - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(CountryController),
                Action = nameof(GetCountriesForDropdown),
                Briefing = "Get the list of all countries.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var result = await _countryService.GetCompactCountries();
            
            return new JsonResult(new { Result = HidroEnums.RESULTS.SUCCESS, Message = result });
        }
    }
}