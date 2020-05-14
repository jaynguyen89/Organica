using Hidrogen.Services.Interfaces;
using MethaneLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("preference")]
    public class PreferenceController {
        
        private readonly ILogger<PreferenceController> _logger;
        private readonly IRuntimeLogService _runtimeLogger;
        private readonly IAccountService _accountService;

        public PreferenceController(
            ILogger<PreferenceController> logger,
            IRuntimeLogService runtimeLogger,
            IAccountService accountService
        ) {
            _logger = logger;
            _runtimeLogger = runtimeLogger;
            _accountService = accountService;
        }
    }
}
