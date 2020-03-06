using HelperLibrary.Interfaces;
using HelperLibrary.ViewModels;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static HelperLibrary.HidroEnums;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("authentication")]
    public class AuthenticationController {

        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authService;
        private readonly IHidrogenianService _userService;
        private readonly IHidroProfileService _profileService;
        private readonly IEmailSenderService _emailService;
        private readonly IGoogleReCaptchaService _googleReCaptchaService;

        private readonly string PROJECT_FOLDER = Path.GetDirectoryName(Directory.GetCurrentDirectory()) + @"/Hidrogen/";

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            IAuthenticationService authService,
            IHidrogenianService userService,
            IHidroProfileService profileService,
            IEmailSenderService emailService,
            IGoogleReCaptchaService googleReCaptchaService
        ) {
            _logger = logger;
            _authService = authService;
            _userService = userService;
            _profileService = profileService;
            _emailService = emailService;
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

        [HttpGet("check-registration-email/{email}")]
        public async Task<JsonResult> CheckRegistrationEmailAvailability(string email) {
            _logger.LogInformation("AuthenticationController.CheckRegistrationEmailAvailability - Service starts.");

            email = email.Trim().ToLower();
            var emailAvailable = await _authService.IsEmailAddressAvailable(email);

            return emailAvailable == null ? new JsonResult(new { Result = RESULTS.FAILED, Error = HTTP_STATUS_CODES.INTERNAL_SERVER_ERROR })
                                          : (emailAvailable.Value ? new JsonResult(new { Result = RESULTS.SUCCESS })
                                                                  : new JsonResult(new { Result = RESULTS.FAILED, Message = "The email you are using has been registered for another account." }));
        }

        [HttpGet("check-registration-username/{username}")]
        public async Task<JsonResult> CheckRegistrationUsernameAvailability(string username) {
            _logger.LogInformation("AuthenticationController.CheckRegistrationUsernameAvailability - Service starts.");

            username = username.Trim();
            var userNameAvailable = await _authService.IsUserNameAvailable(username);

            return userNameAvailable == null ? new JsonResult(new { Result = RESULTS.FAILED, Error = HTTP_STATUS_CODES.INTERNAL_SERVER_ERROR })
                                             : (userNameAvailable.Value ? new JsonResult(new { Result = RESULTS.SUCCESS })
                                                                        : new JsonResult(new { Result = RESULTS.FAILED, Message = "The username you have entered has been taken by a Hidrogenian." }));
        }

        [HttpPost("register-account")]
        public async Task<JsonResult> RegisterAccount(RegistrationVM registration) {
            _logger.LogInformation("AuthenticationController.RegisterAccount - Service starts.");

            //var verification = await _googleReCaptchaService.IsHumanRegistration(registration.CaptchaToken);
            //if (!verification.Result)
            //    return new JsonResult(verification);

            var validation = VerifyRegistrationData(registration);
            if (validation.Count != 0) {
                var messages = registration.GenerateErrorMessages(validation);
                return new JsonResult(new { Result = RESULTS.FAILED, Message = messages });
            }

            var emailAvailable = await _authService.IsEmailAddressAvailable(registration.Email);
            var userNameAvailable = await _authService.IsUserNameAvailable(registration.UserName);
            if (emailAvailable == null || userNameAvailable == null)
                return new JsonResult(new { Result = RESULTS.FAILED, Error = HTTP_STATUS_CODES.INTERNAL_SERVER_ERROR });

            if (!emailAvailable.Value)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "The email you are using has been registered for another account." });

            if (!userNameAvailable.Value)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "The username you have entered has been taken by a Hidrogenian." });

            var encryption = _authService.GenerateHashedPasswordAndSalt(registration.Password);
            registration.Password = encryption.Key;
            registration.PasswordConfirm = encryption.Value;

            var hidrogenian = await _userService.InsertNewHidrogenian(registration);
            if (hidrogenian == null)
                return new JsonResult(new { Result = RESULTS.FAILED, Error = HTTP_STATUS_CODES.INTERNAL_SERVER_ERROR });

            hidrogenian.Token = _authService.GenerateRandomToken();

            if (await _userService.SetAccountConfirmationToken(hidrogenian)) {
                var profile = new HidroProfileVM {
                    HidrogenianId = hidrogenian.Id,
                    FamilyName = hidrogenian.FamilyName,
                    GivenName = hidrogenian.GivenName
                };

                if (await _profileService.InsertProfileForNewlyCreatedHidrogenian(profile)) {
                    string emailTemplate;
                    using (StreamReader reader = File.OpenText(PROJECT_FOLDER + @"HtmlTemplates/AccountActivation.html")) {
                        emailTemplate = reader.ReadToEnd();
                    };

                    emailTemplate = emailTemplate.Replace("[HidrogenianName]", profile.FullName);
                    emailTemplate = emailTemplate.Replace("[HidrogenianEmail]", hidrogenian.Email);
                    emailTemplate = emailTemplate.Replace("[CONFIRM-TOKEN]", hidrogenian.Token);

                    var accountActivationEmail = new EmailParamVM {
                        ReceiverName = profile.FullName,
                        ReceiverAddress = hidrogenian.Email,
                        Subject = "Hidrogenian - Activate your account",
                        Body = emailTemplate
                    };

                    if (await _emailService.SendEmail(accountActivationEmail))
                        return new JsonResult(new { Result = RESULTS.SUCCESS });
                    else {
                        await _userService.RemoveNewlyInsertedHidrogenian(hidrogenian.Id);
                        return new JsonResult(new { Result = RESULTS.INTERRUPTED, Message = "Error occurred while sending Account Activation email." });
                    }
                }
                else {
                    await _userService.RemoveNewlyInsertedHidrogenian(hidrogenian.Id);
                    return new JsonResult(new { Result = RESULTS.INTERRUPTED, Message = "Error occurred while creating User Profile for your account." });
                }
            }
            else
                await _userService.RemoveNewlyInsertedHidrogenian(hidrogenian.Id);

            return new JsonResult(new { Result = RESULTS.FAILED, Error = HTTP_STATUS_CODES.INTERNAL_SERVER_ERROR });
        }

        private List<int> VerifyRegistrationData(RegistrationVM data) {
            _logger.LogInformation("AuthenticationController.VerifyRegistrationData - Verification starts.");

            var errors = data.VerifyEmail();
            errors.AddRange(data.VerifyUserName());
            errors.AddRange(data.VerifyPassword());
            errors.AddRange(data.VerifyFamilyName());
            errors.AddRange(data.VerifyGivenName());

            return errors;
        }
    }
}