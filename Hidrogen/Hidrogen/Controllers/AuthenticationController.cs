using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HelperLibrary;
using HelperLibrary.Common;
using HelperLibrary.Interfaces;
using HelperLibrary.ViewModels;
using Hidrogen.Attributes;
using Hidrogen.Services;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels;
using Hidrogen.ViewModels.Authentication;
using MethaneLibrary.Interfaces;
using MethaneLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static HelperLibrary.HidroEnums;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("authentication")]
    public class AuthenticationController : ControllerBase {

        private readonly ILogger<AuthenticationController> _logger;
        private readonly IRuntimeLogService _runtimeLogger;
        private readonly IAuthenticationService _authService;
        private readonly IHidrogenianService _userService;
        private readonly IHidroProfileService _profileService;
        private readonly IRoleClaimerService _roleClaimer;
        private readonly IEmailSenderService _emailService;
        private readonly IGoogleReCaptchaService _googleReCaptchaService;

        private readonly string PROJECT_FOLDER = Path.GetDirectoryName(Directory.GetCurrentDirectory()) + @"/Hidrogen/";

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            IRuntimeLogService runtimeLogger,
            IAuthenticationService authService,
            IHidrogenianService userService,
            IHidroProfileService profileService,
            IRoleClaimerService roleClaimer,
            IEmailSenderService emailService,
            IGoogleReCaptchaService googleReCaptchaService
        ) {
            _logger = logger;
            _runtimeLogger = runtimeLogger;
            _authService = authService;
            _userService = userService;
            _profileService = profileService;
            _roleClaimer = roleClaimer;
            _emailService = emailService;
            _googleReCaptchaService = googleReCaptchaService;
        }

        public static JsonResult FilterResult(FILTER_RESULT result) {
            return result switch {
                FILTER_RESULT.ACCESS_CONTROL_DENIED => new JsonResult(new {
                    Result = RESULTS.FAILED,
                    Message = "You are not allowed to access this feature. Please login before going further!",
                    Error = HTTP_STATUS_CODES.NONAUTHORITATIVE_INFORMATION
                }),
                FILTER_RESULT.INVALID_AUTHENTICATION => new JsonResult(new {
                    Result = RESULTS.FAILED,
                    Message = "Your session seems to be invalid. Please login again to confirm your identity!",
                    Error = HTTP_STATUS_CODES.PROXY_AUTHENTICATION_REQUIRED
                }),
                FILTER_RESULT.INSUFFICIENT_PERMISSION => new JsonResult(new {
                    Result = RESULTS.FAILED,
                    Message = "You do not have the permission to perform this action. Please close this page.",
                    Error = HTTP_STATUS_CODES.PROXY_AUTHENTICATION_REQUIRED
                }),
                FILTER_RESULT.AUTHENTICATION_EXPIRED => new JsonResult(new {
                    Result = RESULTS.FAILED,
                    Message = "Your session has expired. Please login again to continue!",
                    Error = HTTP_STATUS_CODES.PROXY_AUTHENTICATION_REQUIRED
                }),
                _ => new JsonResult(new {
                    Result = RESULTS.FAILED,
                    Message = "Unable to verify the authenticity of your requests. Please login again.",
                    Error = HTTP_STATUS_CODES.PROXY_AUTHENTICATION_REQUIRED
                })
            };
        }

        [HttpGet("check-registration-email/{email}")]
        [HidroActionFilter]
        public async Task<JsonResult> CheckRegistrationEmailAvailability(string email) {
            _logger.LogInformation("AuthenticationController.CheckRegistrationEmailAvailability - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationController),
                Action = nameof(CheckRegistrationEmailAvailability),
                Data = email,
                Briefing = "Check if the email is available for registration.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            email = email.Trim().ToLower();
            var emailAvailable = await _authService.IsEmailAddressAvailable(email);

            return emailAvailable == null ? new JsonResult(new { Result = RESULTS.FAILED, Error = HTTP_STATUS_CODES.INTERNAL_SERVER_ERROR })
                                          : (emailAvailable.Value ? new JsonResult(new { Result = RESULTS.SUCCESS })
                                                                  : new JsonResult(new { Result = RESULTS.FAILED, Message = "The email you are using has been registered for another account." }));
        }

        [HttpGet("check-registration-username/{username}")]
        [HidroActionFilter]
        public async Task<JsonResult> CheckRegistrationUsernameAvailability(string username) {
            _logger.LogInformation("AuthenticationController.CheckRegistrationUsernameAvailability - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationController),
                Action = nameof(CheckRegistrationUsernameAvailability),
                Data = username,
                Briefing = "Check if the username is available for registration.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            username = username.Trim();
            var userNameAvailable = await _authService.IsUserNameAvailable(username);

            return userNameAvailable == null ? new JsonResult(new { Result = RESULTS.FAILED, Error = HTTP_STATUS_CODES.INTERNAL_SERVER_ERROR })
                                             : (userNameAvailable.Value ? new JsonResult(new { Result = RESULTS.SUCCESS })
                                                                        : new JsonResult(new { Result = RESULTS.FAILED, Message = "The username you have entered has been taken by a Hidrogenian." }));
        }

        [HttpPost("register-account")]
        [HidroActionFilter]
        public async Task<JsonResult> RegisterAccount(RegistrationVM registration) {
            _logger.LogInformation("AuthenticationController.RegisterAccount - Service starts.");
            
            var clone = registration;
            clone.Password = null;
            clone.PasswordConfirm = null;
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationController),
                Action = nameof(RegisterAccount),
                Data = JsonConvert.SerializeObject(clone),
                Briefing = "Create an account for new user then send activation email.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

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

            var (key, value) = _authService.GenerateHashedPasswordAndSalt(registration.Password);
            registration.Password = key;
            registration.PasswordConfirm = value;

            var hidrogenian = await _userService.InsertNewHidrogenian(registration);
            if (hidrogenian == null)
                return new JsonResult(new { Result = RESULTS.FAILED, Error = HTTP_STATUS_CODES.INTERNAL_SERVER_ERROR });

            if (!(await _roleClaimer.SetRoleOnRegistrationFor(hidrogenian.Id))) {
                await _userService.RemoveNewlyInsertedHidrogenian(hidrogenian.Id);
                return new JsonResult(new { Result = RESULTS.INTERRUPTED, Message = "Sorry, error occurred while creating your new account. Please try again." });
            }

            hidrogenian.Token = _authService.GenerateRandomToken();

            if (await _userService.SetAccountConfirmationToken(hidrogenian)) {
                var profile = new HidroProfileVM {
                    HidrogenianId = hidrogenian.Id,
                    FamilyName = hidrogenian.FamilyName,
                    GivenName = hidrogenian.GivenName
                };

                if (await _profileService.InsertProfileForNewlyCreatedHidrogenian(profile)) {
                    string emailTemplate;
                    using (var reader = System.IO.File.OpenText(PROJECT_FOLDER + @"HtmlTemplates/AccountActivation.html")) {
                        emailTemplate = await reader.ReadToEndAsync();
                    }

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
                    await _userService.RemoveNewlyInsertedHidrogenian(hidrogenian.Id);
                    return new JsonResult(new { Result = RESULTS.INTERRUPTED, Message = "Error occurred while sending Account Activation email." });
                }

                await _userService.RemoveNewlyInsertedHidrogenian(hidrogenian.Id);
                return new JsonResult(new { Result = RESULTS.INTERRUPTED, Message = "Error occurred while creating User Profile for your account." });
            }

            await _userService.RemoveNewlyInsertedHidrogenian(hidrogenian.Id);

            return new JsonResult(new { Result = RESULTS.FAILED, Error = HTTP_STATUS_CODES.INTERNAL_SERVER_ERROR });
        }

        [HttpPost("activate-account")]
        [HidroActionFilter]
        public async Task<JsonResult> ActivateAccount(AccountActivationVM activator) {
            _logger.LogInformation("AuthenticationController.ActivateAccount - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationController),
                Action = nameof(ActivateAccount),
                Data = JsonConvert.SerializeObject(activator),
                Briefing = "Activate user account.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var verification = await _googleReCaptchaService.IsHumanRegistration(activator.CaptchaToken);
            if (!verification.Result)
                return new JsonResult(verification);

            var (key, value) = await _authService.ActivateHidrogenianAccount(activator);

            if (!key)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "No Hidrogenian account matches the activation data." });

            if (value == null)
                return new JsonResult(new { Result = RESULTS.FAILED, Error = HTTP_STATUS_CODES.INTERNAL_SERVER_ERROR });

            if (!value.Value)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "The activation data have been no longer valid. Please request another activation email." });

            var fullName = (await _userService.GetHidrogenianByEmail(activator.Email)).FullName;

            string emailTemplate;
            using (var reader = System.IO.File.OpenText(PROJECT_FOLDER + @"HtmlTemplates/AccountActivationConfirmation.html")) {
                emailTemplate = await reader.ReadToEndAsync();
            }

            emailTemplate = emailTemplate.Replace("[HidrogenianName]", fullName);
            var activationConfirmEmail = new EmailParamVM {
                ReceiverName = fullName,
                ReceiverAddress = activator.Email,
                Subject = "Hidrogen - Account Activated",
                Body = emailTemplate
            };

            if (await _emailService.SendEmail(activationConfirmEmail))
                return new JsonResult(new { Result = RESULTS.SUCCESS });

            return new JsonResult(new { Result = RESULTS.INTERRUPTED, Message = "Your account has been activated. However, a confirmation email was failed to send. You can now login to use Hidrogen." });
        }

        [HttpPost("forgot-password")]
        [HidroActionFilter]
        public async Task<JsonResult> SendRecoverPasswordInstruction(RecoveryVM recoveree) {
            _logger.LogInformation("AuthenticationController.RecoverPassword - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationController),
                Action = nameof(SendRecoverPasswordInstruction),
                Data = JsonConvert.SerializeObject(recoveree),
                Briefing = "Set password reset data then send an email to help user reset password.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var verification = await _googleReCaptchaService.IsHumanRegistration(recoveree.CaptchaToken);
            if (!verification.Result)
                return new JsonResult(verification);

            var result = await _authService.SetTempPasswordAndRecoveryToken(recoveree);

            if (result.Key == null)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "No Hidrogenian account matches the provided email address." });

            if (string.IsNullOrEmpty(result.Key))
                return new JsonResult(new { Result = RESULTS.FAILED, Error = HTTP_STATUS_CODES.INTERNAL_SERVER_ERROR });

            string emailTemplate;
            using (var reader = System.IO.File.OpenText(PROJECT_FOLDER + @"HtmlTemplates/PasswordReset.html")) {
                emailTemplate = await reader.ReadToEndAsync();
            }

            var fullName = (await _userService.GetHidrogenianByEmail(recoveree.Email)).FullName;

            emailTemplate = emailTemplate.Replace("[HidrogenianName]", fullName);
            emailTemplate = emailTemplate.Replace("[HidrogenianEmail]", recoveree.Email);
            emailTemplate = emailTemplate.Replace("[INITIAL-PW]", result.Key);
            emailTemplate = emailTemplate.Replace("[CONFIRM-TOKEN]", result.Value);

            var recoverPasswordEmail = new EmailParamVM {
                ReceiverName = fullName,
                ReceiverAddress = recoveree.Email,
                Subject = "Hidrogen - Reset your password",
                Body = emailTemplate
            };

            if (await _emailService.SendEmail(recoverPasswordEmail))
                return new JsonResult(new { Result = RESULTS.SUCCESS });

            return new JsonResult(new { Result = RESULTS.FAILED, Message = "Email was failed to send. Please try again." });
        }

        [HttpPost("request-new-activation-email")]
        [HidroActionFilter]
        public async Task<JsonResult> SendNewAccountActivationEmail(RecoveryVM request) {
            _logger.LogInformation("AuthenticationController.SendNewAccountActivationEmail - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationController),
                Action = nameof(SendNewAccountActivationEmail),
                Data = JsonConvert.SerializeObject(request),
                Briefing = "Set account activation data then send another email to help user activate account.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var verification = await _googleReCaptchaService.IsHumanRegistration(request.CaptchaToken);
            if (!verification.Result)
                return new JsonResult(verification);

            var hidrogenian = await _userService.GetUnactivatedHidrogenianByEmail(request.Email);
            if (hidrogenian == null)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "No Hidrogenian account matches the provided email address. Otherwise, if you have an Account Activation email that has not expired, please follow instruction in the email." });

            hidrogenian.Token = _authService.GenerateRandomToken();

            if (!await _userService.SetAccountConfirmationToken(hidrogenian))
                return new JsonResult(new {Result = RESULTS.FAILED, Error = HTTP_STATUS_CODES.INTERNAL_SERVER_ERROR});
            
            string emailTemplate;
            using (var reader = System.IO.File.OpenText(PROJECT_FOLDER + @"HtmlTemplates/AccountActivation.html")) {
                emailTemplate = await reader.ReadToEndAsync();
            }

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

        [HttpPost("set-new-password")]
        [HidroActionFilter]
        public async Task<JsonResult> SetNewPassword(RegistrationVM recovery) {
            _logger.LogInformation("AuthenticationController.SetNewPassword - Service starts.");

            var clone = recovery;
            clone.Password = null;
            clone.PasswordConfirm = null;
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationController),
                Action = nameof(SetNewPassword),
                Data = JsonConvert.SerializeObject(clone),
                Briefing = "Set new password on recovery and send confirmation email.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

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
            using (var reader = System.IO.File.OpenText(PROJECT_FOLDER + @"HtmlTemplates/PasswordResetConfirmation.html")) {
                emailTemplate = await reader.ReadToEndAsync();
            }

            var fullName = (await _userService.GetHidrogenianByEmail(recovery.Email)).FullName;
            emailTemplate = emailTemplate.Replace("[HidrogenianName]", fullName);

            var accountActivationEmail = new EmailParamVM {
                ReceiverName = fullName,
                ReceiverAddress = recovery.Email,
                Subject = "Hidrogen - New password has set",
                Body = emailTemplate
            };

            if (await _emailService.SendEmail(accountActivationEmail))
                return new JsonResult(new { Result = RESULTS.SUCCESS });

            return new JsonResult(new { Result = RESULTS.INTERRUPTED, Message = "Your new password has been updated successfully. However, a confirmation email was failed to send." });
        }

        [HttpPost("authenticate")]
        [HidroActionFilter]
        public async Task<JsonResult> Authenticate(AuthenticationVM auth) {
            _logger.LogInformation("AuthenticationController.Authenticate - Service starts.");

            var clone = auth;
            clone.Password = null;
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationController),
                Action = nameof(Authenticate),
                Data = JsonConvert.SerializeObject(clone),
                Briefing = "Authenticate user using login form data.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            HttpContext.Session.Clear();

            //var verification = await _googleReCaptchaService.IsHumanRegistration(auth.CaptchaToken);
            //if (!verification.Result)
                //return new JsonResult(verification);

            var validation = auth.VerifyAuthenticationData();
            if (validation.Count != 0) {
                var message = auth.GenerateErrorMessages(validation);
                return new JsonResult(new { Result = RESULTS.FAILED, Message = message });
            }

            var (key, value) = await _authService.AuthenticateHidrogenian(auth);
            if (!key)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "No Hidrogenian account matches the provided email address." });

            if (value == null)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "Cannot find any Hidrogenian with the login credentials." });

            await SetUserSessionAndCookie(value, auth.TrustedAuth);
            if (await SetUserAuthorizationPolicy(value.UserId))
                return new JsonResult(new { Result = RESULTS.SUCCESS, Message = value });

            return new JsonResult(new { Result = RESULTS.FAILED, Message = "Error occurred while sign into your account. Please try again." });
        }

        [HttpPost("cookie-authenticate")]
        [HidroActionFilter]
        public async Task<JsonResult> CookieAuthenticate(CookieAuthenticationVM cookie) {
            _logger.LogInformation("AuthenticationController.CookieAuthenticate - Service starts.");

            var clone = cookie;
            clone.CookieToken = null;
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationController),
                Action = nameof(CookieAuthenticate),
                Data = JsonConvert.SerializeObject(clone),
                Briefing = "Authenticate user using cookie data.",
                Severity = LOGGING.INFORMATION.GetValue()
            });
            
            HttpContext.Session.Clear();

            var (key, value) = await _authService.AuthenticateWithCookie(cookie);
            if (!key) return new JsonResult(new { Result = RESULTS.FAILED });

            await SetUserSessionAndCookie(value, cookie.TrustedAuth == "True");
            if (await SetUserAuthorizationPolicy(value.UserId))
                return new JsonResult(new { Result = RESULTS.SUCCESS, Message = value });

            return new JsonResult(new { Result = RESULTS.FAILED, Message = "Error occurred while sign into your account. Please try to manually login." });
        }

        private async Task SetUserSessionAndCookie(AuthenticatedUser authHidrogenian, bool trusted) {
            _logger.LogInformation("AuthenticationController.SetUserSessionAndCookie - private action.");

            var clone = authHidrogenian;
            clone.AuthToken = null;
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationController),
                Action = "private " + nameof(SetUserSessionAndCookie),
                Data = JsonConvert.SerializeObject(clone),
                Briefing = "Internally set user auth data to HttpContext.Session and send to client over Response.Cookie.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            HttpContext.Session.SetString(nameof(AuthenticatedUser.AuthToken), authHidrogenian.AuthToken);
            HttpContext.Session.SetInt32(nameof(AuthenticatedUser.ExpirationTime), (int)authHidrogenian.ExpirationTime);
            HttpContext.Session.SetInt32(nameof(AuthenticatedUser.UserId), authHidrogenian.UserId);
            HttpContext.Session.SetString(nameof(AuthenticatedUser.Role), authHidrogenian.Role);

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

        private async Task<bool> SetUserAuthorizationPolicy(int hidrogenianId) {
            _logger.LogInformation("AuthenticationController.SetUserAuthorizationPolicy - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationController),
                Action = "private " + nameof(SetUserAuthorizationPolicy),
                Data = hidrogenianId.ToString(),
                Briefing = "Internally set user permissions to HttpContext.Session.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var permissions = await _authService.ComputeAuthorizationFor(hidrogenianId);
            if (permissions == null) return false;

            HttpContext.Session.SetString(nameof(HidroAuthorize), JsonConvert.SerializeObject(permissions));
            return true;
        }

        [HttpGet("sign-out")]
        [HidroActionFilter]
        [HidroAuthorize("0,1,0,0,0,0,0,0")]
        public JsonResult LogOut() {
            _logger.LogInformation("AuthenticationController.LogOut - Service starts.");
            _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationController),
                Action = nameof(LogOut),
                Briefing = "Endpoint invoked - Logout user.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            HttpContext.Session.Clear();

            return new JsonResult(new { Result = RESULTS.SUCCESS });
        }

        public void LogOutInternal() {
            _logger.LogInformation("AuthenticationController.LogOutInternal - Service runs internally.");
            HttpContext.Session.Clear();
        }

        private List<int> VerifyRegistrationData(RegistrationVM data) {
            _logger.LogInformation("AuthenticationController.VerifyRegistrationData - Verification starts.");

            var clone = data;
            clone.Password = null;
            clone.PasswordConfirm = null;
            _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationController),
                Action = "private " + nameof(VerifyRegistrationData),
                Data = JsonConvert.SerializeObject(clone),
                Briefing = "Internally check if registration data have any errors.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var errors = data.VerifyEmail();
            errors.AddRange(data.VerifyUserName());
            errors.AddRange(data.VerifyPassword());
            errors.AddRange(data.VerifyFamilyName());
            errors.AddRange(data.VerifyGivenName());

            return errors;
        }
    }
}