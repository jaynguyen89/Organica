using HelperLibrary.Common;
using HelperLibrary.Interfaces;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static HelperLibrary.HidroEnums;

namespace Hidrogen.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class HidrogenianController {

        private readonly ILogger<HidrogenianController> _logger;
        private readonly IAuthenticationService _authService;
        private readonly IHidrogenianService _userService;
        private readonly IGoogleReCaptchaService _googleReCaptchaService;
        private readonly HttpClient GoogleCaptchaRequest = new HttpClient();

        public HidrogenianController(
            ILogger<HidrogenianController> logger,
            IAuthenticationService authService,
            IHidrogenianService userService,
            IGoogleReCaptchaService googleReCaptchaService
        ) {
            _logger = logger;
            _authService = authService;
            _userService = userService;
            _googleReCaptchaService = googleReCaptchaService;
        }

        public JsonResult FilterResult(string result) {
            switch (result) {
                case "Unauthenticated":
                    return new JsonResult(new {
                        Result = RESULTS.FAILED,
                        Message = "You are not allowed to access this feature. Please login before going further!",
                        Error = HTTP_STATUS_CODES.NONAUTHORITATIVE_INFORMATION
                    });
                case "InvalidAuthentication":
                    return new JsonResult(new {
                        Result = RESULTS.FAILED,
                        Message = "Your session seems to be invalid. Please login again to confirm your identity!",
                        Error = HTTP_STATUS_CODES.PROXY_AUTHENTICATION_REQUIRED
                    });
                case "AuthenticationExpired":
                    return new JsonResult(new {
                        Result = RESULTS.FAILED,
                        Message = "Your session has expired. Please login again to continue!",
                        Error = HTTP_STATUS_CODES.PROXY_AUTHENTICATION_REQUIRED
                    });
                case "AccessControlDenied":
                    return new JsonResult(new {
                        Result = RESULTS.FAILED,
                        Message = "You are required to have another role to access this page!",
                        Error = HTTP_STATUS_CODES.UNAUTHORIZED
                    });
                default: //NoAuthentication
                    return new JsonResult(new {
                        Result = RESULTS.FAILED,
                        Message = "Unable to authenticate your user credentials. Please login again.",
                        Error = HTTP_STATUS_CODES.NONAUTHORITATIVE_INFORMATION
                    });
            }
        }

        [HttpPost("RegisterAccount")]
        public async Task<JsonResult> RegisterAccount(HidrogenianVM registrationData) {
            _logger.LogInformation("HidrogenianController.RegisterAccount - Service starts.");

            var verification = await _googleReCaptchaService.IsHumanRegistration(registrationData.CaptchaToken);
            if (!verification.Result)
                return new JsonResult(verification);



            return null;
        }
    }
}