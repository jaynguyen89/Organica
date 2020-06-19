using System.Threading.Tasks;
using HelperLibrary;
using Hidrogen.Attributes;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels;
using MethaneLibrary.Interfaces;
using MethaneLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Hidrogen.Controllers {
    
    [ApiController]
    [Route("country")]
    public class CountryController : AppController {
        
        private readonly ILogger<CountryController> _logger;
        private readonly IRuntimeLogService _runtimeLogger;
        private readonly ICountryService _countryService;

        public CountryController(
            ILogger<CountryController> logger,
            IRuntimeLogService runtimeLogger,
            ICountryService countryService,
            IDistributedCache redisCache
        ) : base(redisCache) {
            _logger = logger;
            _runtimeLogger = runtimeLogger;
            _countryService = countryService;
        }
        
        [HttpGet("compact-countries")]
        [HidroActionFilter(HidroEnums.ROLES.CUSTOMER)]
        [HidroAuthorize(HidroEnums.PERMISSIONS.VIEW)]
        public async Task<JsonResult> GetCountriesForDropdown() {
            _logger.LogInformation("CountryController.GetCountriesForDropdown - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(CountryController),
                Action = nameof(GetCountriesForDropdown),
                Briefing = "Get the list of all countries.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var compactCountries = await ReadFromRedisCacheAsync<CountryVM[]>("Country_CompactList", true);
            if (compactCountries != null) return new JsonResult(new { Result = HidroEnums.RESULTS.SUCCESS, Message = compactCountries });

            compactCountries = await _countryService.GetCompactCountries();
            
            await InsertRedisCacheEntryAsync("Country_CompactList", compactCountries, true);
            return new JsonResult(new { Result = HidroEnums.RESULTS.SUCCESS, Message = compactCountries });
        }
    }
}