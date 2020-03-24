using Hidrogen.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("profile")]
    public class AccountController {

        public readonly ILogger<AccountController> _logger;
        public readonly IAccountService _accountService;

        private readonly string PROJECT_FOLDER = Path.GetDirectoryName(Directory.GetCurrentDirectory()) + @"/Hidrogen/";

        public AccountController(
            ILogger<AccountController> logger,
            IAccountService accountService
        ) {
            _logger = logger;
            _accountService = accountService;
        }
    }
}
