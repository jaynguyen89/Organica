using System.Threading.Tasks;
using HelperLibrary;
using HelperLibrary.Common;
using Hidrogen.Attributes;
using Hidrogen.Services;
using MethaneLibrary.Interfaces;
using MethaneLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WaterLibrary.Interfaces;
using WaterLibrary.ViewModels;
using static HelperLibrary.HidroEnums;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("water")]
    public class WaterController {

        private readonly ILogger<WaterController> _logger;
        private readonly IRuntimeLogService _runtimeLogger;
        private readonly IWaterService _tokenService;

        private const int TOKEN_LIFE = 3600; // 3 Minutes

        public WaterController(
            ILogger<WaterController> logger,
            IRuntimeLogService runtimeLogger,
            IWaterService tokenService
        ) {
            _logger = logger;
            _runtimeLogger = runtimeLogger;
            _tokenService = tokenService;
        }

        [HttpGet("get-api-key/{task}")]
        [HidroActionFilter(ROLES.CUSTOMER)]
        [HidroAuthorize(PERMISSIONS.CREATE)]
        public async Task<JsonResult> GetApiToken(string task) {
            _logger.LogInformation("WaterController.GetApiToken - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(WaterController),
                Action = nameof(GetApiToken),
                Briefing = "Get a WATER API token before uploading file to Water for task = " + task,
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var tokenLength = HelperProvider.RandomNumberInRange(30, 100);

            var token = new TokenVM {
                Token = HelperProvider.GenerateRandomString(tokenLength),
                Duration = TOKEN_LIFE,
                Target = HidroConstants.API_TOKEN_TARGETS[task]
            };

            var result = await _tokenService.SetApiToken(token);
            if (!result) return new JsonResult(new { Result = RESULTS.FAILED, Message = "An error occurred while attempting to load your photos. Please reload page to try again." });

            return new JsonResult(new { Result = RESULTS.SUCCESS, Message = token.Token });
        }
    }
}
