using Hidrogen.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("profile")]
    public class AccountController {

        public readonly ILogger<AccountController> _logger;
        public readonly IAccountService _accountService;

        public AccountController(
            ILogger<AccountController> logger,
            IAccountService accountService
        ) {
            _logger = logger;
            _accountService = accountService;
        }
    }
}
