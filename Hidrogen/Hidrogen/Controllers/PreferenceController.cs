using Hidrogen.Services.Interfaces;
using MethaneLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("preference")]
    public class PreferenceController : AppController {
        
        private readonly ILogger<PreferenceController> _logger;
        private readonly IRuntimeLogService _runtimeLogger;
        private readonly IAccountService _accountService;

        public PreferenceController(
            ILogger<PreferenceController> logger,
            IRuntimeLogService runtimeLogger,
            IAccountService accountService,
            IDistributedCache redisCache
        ) : base(redisCache) {
            _logger = logger;
            _runtimeLogger = runtimeLogger;
            _accountService = accountService;
        }
    }
}
