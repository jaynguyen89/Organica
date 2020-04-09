using System.Threading.Tasks;
using HelperLibrary;
using HelperLibrary.Common;
using Hidrogen.Attributes;
using Hidrogen.Services;
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
        private readonly IWaterService _tokenService;

        private const int TOKEN_LIFE = 3600; // 3 Minutes

        public WaterController(
            ILogger<WaterController> logger,
            IWaterService tokenService
        ) {
            _logger = logger;
            _tokenService = tokenService;
        }

        [HttpGet("get-api-key/{task}")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("1,0,0,0,0,0,0,0")]
        public async Task<JsonResult> GetApiToken(string task) {
            _logger.LogInformation("WaterController.GetApiToken - Service starts.");

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
