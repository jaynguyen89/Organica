using HelperLibrary.Common;
using HelperLibrary.Interfaces;
using HelperLibrary.ViewModels;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static HelperLibrary.HidroEnums;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("authentication")]
    public class AuthenticationController : ControllerBase {

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
                        Message = "Your request failed to pass the authenticity validation. Please login again.",
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

            var verification = await _googleReCaptchaService.IsHumanRegistration(registration.CaptchaToken);
            if (!verification.Result)
                return new JsonResult(verification);

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
                    using (StreamReader reader = System.IO.File.OpenText(PROJECT_FOLDER + @"HtmlTemplates/AccountActivation.html")) {
                        emailTemplate = reader.ReadToEnd();
                    };

                    emailTemplate = emailTemplate.Replace("[HidrogenianName]", profile.FullName);
                    emailTemplate = emailTemplate.Replace("[HidrogenianEmail]", hidrogenian.Email);
                    emailTemplate = emailTemplate.Replace("[CONFIRM-TOKEN]", hidrogenian.Token);

                    var accountActivationEmail = new EmailParamVM {
                        ReceiverName = profile.FullName,
                        ReceiverAddress = hidrogenian.Email,
                        Subject = "Hidrogen - Activate your account",
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

        [HttpPost("activate-account")]
        public async Task<JsonResult> ActivateAccount(AccountActivationVM activator) {
            _logger.LogInformation("AuthenticationController.ActivateAccount - Service starts.");

            var verification = await _googleReCaptchaService.IsHumanRegistration(activator.CaptchaToken);
            if (!verification.Result)
                return new JsonResult(verification);

            var result = await _authService.ActivateHidrogenianAccount(activator);

            if (!result.Key)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "No Hidrogenian account matches the activation data." });

            if (result.Value == null)
                return new JsonResult(new { Result = RESULTS.FAILED, Error = HTTP_STATUS_CODES.INTERNAL_SERVER_ERROR });

            if (!result.Value.Value)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "The activation data have been no longer valid. Please request another activation email." });

            var FullName = (await _userService.GetHidrogenianByEmail(activator.Email)).FullName;

            string emailTemplate;
            using (StreamReader reader = System.IO.File.OpenText(PROJECT_FOLDER + @"HtmlTemplates/AccountActivationConfirmation.html")) {
                emailTemplate = reader.ReadToEnd();
            };

            emailTemplate = emailTemplate.Replace("[HidrogenianName]", FullName);
            var activationConfirmEmail = new EmailParamVM {
                ReceiverName = FullName,
                ReceiverAddress = activator.Email,
                Subject = "Hidrogen - Account Activated",
                Body = emailTemplate
            };

            if (await _emailService.SendEmail(activationConfirmEmail))
                return new JsonResult(new { Result = RESULTS.SUCCESS });

            return new JsonResult(new { Result = RESULTS.INTERRUPTED, Message = "Your account has been activated. However, a confirmation email was failed to send. You can now login to use Hidrogen." });
        }

        [HttpPost("forgot-password")]
        public async Task<JsonResult> SendRecoverPasswordInstruction(RecoveryVM recoveree) {
            _logger.LogInformation("AuthenticationController.RecoverPassword - Service starts.");

            var verification = await _googleReCaptchaService.IsHumanRegistration(recoveree.CaptchaToken);
            if (!verification.Result)
                return new JsonResult(verification);

            var result = await _authService.SetTempPasswordAndRecoveryToken(recoveree);

            if (result.Key == null)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "No Hidrogenian account matches the provided email address." });

            if (string.IsNullOrEmpty(result.Key))
                return new JsonResult(new { Result = RESULTS.FAILED, Error = HTTP_STATUS_CODES.INTERNAL_SERVER_ERROR });

            string emailTemplate;
            using (StreamReader reader = System.IO.File.OpenText(PROJECT_FOLDER + @"HtmlTemplates/PasswordReset.html")) {
                emailTemplate = reader.ReadToEnd();
            };

            var FullName = (await _userService.GetHidrogenianByEmail(recoveree.Email)).FullName;

            emailTemplate = emailTemplate.Replace("[HidrogenianName]", FullName);
            emailTemplate = emailTemplate.Replace("[HidrogenianEmail]", recoveree.Email);
            emailTemplate = emailTemplate.Replace("[INITIAL-PW]", result.Key);
            emailTemplate = emailTemplate.Replace("[CONFIRM-TOKEN]", result.Value);

            var recoverPasswordEmail = new EmailParamVM {
                ReceiverName = FullName,
                ReceiverAddress = recoveree.Email,
                Subject = "Hidrogen - Reset your password",
                Body = emailTemplate
            };

            if (await _emailService.SendEmail(recoverPasswordEmail))
                return new JsonResult(new { Result = RESULTS.SUCCESS });

            return new JsonResult(new { Result = RESULTS.FAILED, Message = "Email was failed to send. Please try again." });
        }

        [HttpPost("request-new-activation-email")]
        public async Task<JsonResult> SendNewAccountActivationEmail(RecoveryVM request) {
            _logger.LogInformation("AuthenticationController.SendNewAccountActivationEmail - Service starts.");

            var verification = await _googleReCaptchaService.IsHumanRegistration(request.CaptchaToken);
            if (!verification.Result)
                return new JsonResult(verification);

            var hidrogenian = await _userService.GetUnactivatedHidrogenianByEmail(request.Email);
            if (hidrogenian == null)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "No Hidrogenian account matches the provided email address. Otherwise, if you have an Account Activation email that has not expired, please follow instruction in the email." });

            hidrogenian.Token = _authService.GenerateRandomToken();

            if (await _userService.SetAccountConfirmationToken(hidrogenian)) {
                string emailTemplate;
                using (StreamReader reader = System.IO.File.OpenText(PROJECT_FOLDER + @"HtmlTemplates/AccountActivation.html")) {
                    emailTemplate = reader.ReadToEnd();
                };

                emailTemplate = emailTemplate.Replace("[HidrogenianName]", hidrogenian.FullName);
                emailTemplate = emailTemplate.Replace("[HidrogenianEmail]", hidrogenian.Email);
                emailTemplate = emailTemplate.Replace("[CONFIRM-TOKEN]", hidrogenian.Token);

                var accountActivationEmail = new EmailParamVM {
                    ReceiverName = hidrogenian.FullName,
                    ReceiverAddress = hidrogenian.Email,
                    Subject = "Hidrogen - Activate your account",
                    Body = emailTemplate
                };

                if (await _emailService.SendEmail(accountActivationEmail))
                    return new JsonResult(new { Result = RESULTS.SUCCESS });

                return new JsonResult(new { Result = RESULTS.FAILED, Message = "Unable to send Account Activation email at the moment. Please try again." });
            }

            return new JsonResult(new { Result = RESULTS.FAILED, Error = HTTP_STATUS_CODES.INTERNAL_SERVER_ERROR });
        }

        [HttpPost("set-new-password")]
        public async Task<JsonResult> SetNewPassword(RegistrationVM recovery) {
            _logger.LogInformation("AuthenticationController.SetNewPassword - Service starts.");

            var verification = await _googleReCaptchaService.IsHumanRegistration(recovery.CaptchaToken);
            if (!verification.Result)
                return new JsonResult(verification);

            var validation = recovery.VerifyPassword();
            if (validation.Count != 0) {
                var messages = recovery.GenerateErrorMessages(validation);
                return new JsonResult(new { Result = RESULTS.FAILED, Message = messages });
            }

            var result = await _authService.ReplaceAccountPassword(recovery);

            if (result == null)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "No Hidrogenian account matches the provided email address." });

            if (!result.Value.Key)
                return new JsonResult(new { Result = RESULTS.FAILED, Messsage = "The recovery data have been no longer valid. Please request another Password Recovery email." });

            if (result.Value.Value == null)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "The temporary password was invalid. Please check and submit again." });

            if (!result.Value.Value.Value)
                return new JsonResult(new { Result = RESULTS.FAILED, Error = HTTP_STATUS_CODES.INTERNAL_SERVER_ERROR });

            string emailTemplate;
            using (StreamReader reader = System.IO.File.OpenText(PROJECT_FOLDER + @"HtmlTemplates/PasswordResetConfirmation.html")) {
                emailTemplate = reader.ReadToEnd();
            };

            var FullName = (await _userService.GetHidrogenianByEmail(recovery.Email)).FullName;
            emailTemplate = emailTemplate.Replace("[HidrogenianName]", FullName);

            var accountActivationEmail = new EmailParamVM {
                ReceiverName = FullName,
                ReceiverAddress = recovery.Email,
                Subject = "Hidrogen - New password has set",
                Body = emailTemplate
            };

            if (await _emailService.SendEmail(accountActivationEmail))
                return new JsonResult(new { Result = RESULTS.SUCCESS });

            return new JsonResult(new { Result = RESULTS.INTERRUPTED, Message = "Your new password has been updated successfully. However, a confirmation email was failed to send." });
        }

        [HttpPost("authenticate")]
        public async Task<JsonResult> Authenticate(AuthenticationVM auth) {
            _logger.LogInformation("AuthenticationController.Authenticate - Service starts.");

            var verification = await _googleReCaptchaService.IsHumanRegistration(auth.CaptchaToken);
            if (!verification.Result)
                return new JsonResult(verification);

            var validation = auth.VerifyAuthenticationData();
            if (validation.Count != 0) {
                var message = auth.GenerateErrorMessages(validation);
                return new JsonResult(new { Result = RESULTS.FAILED, Message = message });
            }

            var authResult = await _authService.AuthenticateHidrogenian(auth);
            if (!authResult.Key)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "No Hidrogenian account matches the provided email address." });

            if (authResult.Value == null)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "Cannot find any Hidrogenian with the login credentials." });

            await SetUserSessionAndCookie(authResult.Value, auth.TrustedAuth);
            return new JsonResult(new { Result = RESULTS.SUCCESS, Message = authResult.Value });
        }

        [HttpPost("cookie-authenticate")]
        public async Task<JsonResult> CookieAuthenticate(CookieAuthenticationVM cookie) {
            _logger.LogInformation("AuthenticationController.CookieAuthenticate - Service starts.");

            var result = await _authService.AuthenticateWithCookie(cookie);
            if (!result.Key) return new JsonResult(new { Result = RESULTS.FAILED });

            await SetUserSessionAndCookie(result.Value, cookie.TrustedAuth == "True");
            return new JsonResult(new { Result = RESULTS.SUCCESS, Message = result.Value });
        }

        private async Task SetUserSessionAndCookie(AuthenticatedUser authHidrogenian, bool trusted) {
            _logger.LogInformation("AuthenticationController.SetUserSessionAndCookie - private action.");

            HttpContext.Session.SetString(nameof(authHidrogenian.AuthToken), authHidrogenian.AuthToken);
            HttpContext.Session.SetInt32(nameof(authHidrogenian.ExpirationTime), authHidrogenian.ExpirationTime);
            HttpContext.Session.SetInt32(nameof(authHidrogenian.UserId), authHidrogenian.UserId);
            HttpContext.Session.SetString(nameof(authHidrogenian.Role), authHidrogenian.Role);

            var cookieOptions = new CookieOptions {
                HttpOnly = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMilliseconds(HidroConstants.CLIENT_COOKIE_EXPIRATION_TIME),
                Domain = "localhost"
            };

            var cookieAuth = await _authService.GenerateCookieAuthData(authHidrogenian);
            if (cookieAuth != null) {
                Response.Cookies.Append("HidrogenianAuthCookie", cookieAuth.CookieToken, cookieOptions);
                Response.Cookies.Append("AuthCookieTimeStamp", cookieAuth.TimeStamp.ToString(), cookieOptions);
                Response.Cookies.Append("TrustedAuth", trusted ? "True" : "False", cookieOptions);
            }
        }

        [HttpGet("sign-out")]
        public JsonResult LogOut() {
            _logger.LogInformation("AuthenticationController.LogOut - Service starts.");

            HttpContext.Session.Clear();

            return new JsonResult(new { Result = RESULTS.SUCCESS });
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