using Hidrogen.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("preference")]
    public class PreferenceController {

        public readonly ILogger<PreferenceController> _logger;
        public readonly IAccountService _accountService;

        public PreferenceController(
            ILogger<PreferenceController> logger,
            IAccountService accountService
        ) {
            _logger = logger;
            _accountService = accountService;
        }
    }
}
