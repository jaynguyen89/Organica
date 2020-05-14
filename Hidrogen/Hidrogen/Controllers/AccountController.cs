using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Google.Authenticator;
using HelperLibrary;
using HelperLibrary.Common;
using HelperLibrary.Interfaces;
using HelperLibrary.ViewModels;
using Hidrogen.Attributes;
using Hidrogen.Services;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels;
using Hidrogen.ViewModels.Account;
using MethaneLibrary.Interfaces;
using MethaneLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static HelperLibrary.HidroEnums;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("account")]
    public class AccountController {
        
        private readonly ILogger<AccountController> _logger;
        private readonly IRuntimeLogService _runtimeLogger;
        private readonly IAccountService _accountService;
        private readonly IAuthenticationService _authService;
        private readonly IHidrogenianService _userService;
        private readonly IHidroProfileService _profileService;
        private readonly IEmailSenderService _emailService;
        private readonly IGoogleReCaptchaService _reCaptchaService;

        private readonly string PROJECT_FOLDER = Path.GetDirectoryName(Directory.GetCurrentDirectory()) + @"/Hidrogen/";

        public AccountController(
            ILogger<AccountController> logger,
            IRuntimeLogService runtimeLogger,
            IAccountService accountService,
            IAuthenticationService authService,
            IHidrogenianService userService,
            IHidroProfileService profileService,
            IEmailSenderService emailService,
            IGoogleReCaptchaService reCaptchaService
        ) {
            _logger = logger;
            _runtimeLogger = runtimeLogger;
            _accountService = accountService;
            _authService = authService;
            _userService = userService;
            _profileService = profileService;
            _emailService = emailService;
            _reCaptchaService = reCaptchaService;
        }

        [HttpGet("get-identity/{hidrogenianId}")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,1,0,0,0,0,0,0")]
        public async Task<JsonResult> GetIdentityDetailFor(int hidrogenianId) {
            _logger.LogInformation("AccountController.GetIdentityDetailFor - hidrogenianId=" + hidrogenianId);
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AccountController),
                Action = nameof(GetIdentityDetailFor),
                Data = hidrogenianId.ToString(),
                Briefing = "Get data for Identity Panel in CAB.",
                Severity = LOGGING.INFORMATION.GetValue()
            });
            
            var identity = await _accountService.GetAccountIdentity(hidrogenianId);

            return identity == null ? new JsonResult(new { Result = RESULTS.FAILED, Message = "Unable to find an account with the given data. Please reload page to try again." })
                                    : new JsonResult(new { Result = RESULTS.SUCCESS, Message = identity });
        }

        [HttpGet("get-two-fa/{hidrogenianId}")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,1,0,0,0,0,0,0")]
        public async Task<JsonResult> GetTwoFactorDataFor(int hidrogenianId) {
            _logger.LogInformation("AccountController.GetTwoFactorDataFor - hidrogenianId=" + hidrogenianId);
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AccountController),
                Action = nameof(GetTwoFactorDataFor),
                Data = hidrogenianId.ToString(),
                Briefing = "Get 2FA data for Security Panel in CAB.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var secretKey = await _accountService.RetrieveTwoFaSecretKeyFor(hidrogenianId);
            if (secretKey == null) return new JsonResult(new { Result =RESULTS.FAILED, Message = "Error occurred while looking for your Two-Factor Authentication." });

            if (secretKey.Length == 0) return new JsonResult(new { Result = RESULTS.SUCCESS });
            
            var twoFa = new TwoFaVM { Id = hidrogenianId };
            var identity = await _accountService.GetAccountIdentity(hidrogenianId);

            var tfa = new TwoFactorAuthenticator();
            var authenticator = tfa.GenerateSetupCode(
                HidroConstants.PROJECT_NAME, identity.Email,
                secretKey, false, 200
            );

            twoFa.QrImageUrl = authenticator.QrCodeSetupImageUrl;
            twoFa.ManualQrCode = authenticator.ManualEntryKey;

            return new JsonResult(new { Result = RESULTS.SUCCESS, Message = twoFa });
        }

        [HttpGet("get-time-logs/{hidrogenianId}")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,1,0,0,0,0,0,0")]
        public async Task<JsonResult> GetTimeStampsFor(int hidrogenianId) {
            _logger.LogInformation("AccountController.GetAccountTimeStamps - hidrogenianId=" + hidrogenianId);
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AccountController),
                Action = nameof(GetTimeStampsFor),
                Data = hidrogenianId.ToString(),
                Briefing = "Get timestamp data for TimeStamp Panel in CAB.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var timeStamps = await _accountService.GetAccountTimeStamps(hidrogenianId);

            return timeStamps == null ? new JsonResult(new { Result = RESULTS.FAILED, Message = "Failed to retrieve account activity logs. Please reload page to try again." })
                                      : new JsonResult(new { Result = RESULTS.SUCCESS, Message = timeStamps });
        }

        [HttpPost("update-identity")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,0,1,0,0,0,0,0")]
        public async Task<JsonResult> UpdateAccountIdentity(AccountIdentityVM identity) {
            _logger.LogInformation("AccountController.UpdateAccountIdentity - hidrogenianId=" + identity.Id);
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AccountController),
                Action = nameof(UpdateAccountIdentity),
                Data = JsonConvert.SerializeObject(identity),
                Briefing = "Update identity data for an account.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var validation = await _reCaptchaService.IsHumanRegistration(identity.CaptchaToken);
            if (!validation.Result) return new JsonResult(validation);

            var verification = VerifyIdentityData(identity);
            if (verification.Count != 0) {
                var messages = identity.GenerateErrorMessages(verification);
                return new JsonResult(new { Result = RESULTS.FAILED, Message = messages });
            }

            var (key, value) = await _accountService.UpdateIdentityForHidrogenian(identity);

            if (!key)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "Account not found with the given data. Please try again." });

            if (value == null)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "An error occurred while attempting to update your account. Please try again." });

            var oldIdentity = value.Value.Key;
            var newIdentity = value.Value.Value;
            var hidrogenian = new HidrogenianVM {
                Id = newIdentity.Id,
                Token = _authService.GenerateRandomToken()
            };

            if (!newIdentity.EmailConfirmed)
                if (await _userService.SetAccountConfirmationToken(hidrogenian)) {
                    var profile = await _profileService.GetPublicProfileFor(newIdentity.Id);

                    string emailTemplate;
                    using (var reader = File.OpenText(PROJECT_FOLDER + @"HtmlTemplates/EmailChanged.html")) {
                        emailTemplate = await reader.ReadToEndAsync();
                    };

                    emailTemplate = emailTemplate.Replace("[HidrogenianName]", profile.FullName);
                    emailTemplate = emailTemplate.Replace("[HidrogenianEmail]", newIdentity.Email);
                    emailTemplate = emailTemplate.Replace("[CONFIRM-TOKEN]", hidrogenian.Token);

                    var emailChangedEmail = new EmailParamVM {
                        ReceiverName = profile.FullName,
                        ReceiverAddress = hidrogenian.Email,
                        Subject = "Hidrogen - Confirm your email",
                        Body = emailTemplate
                    };

                    if (await _emailService.SendEmail(emailChangedEmail)) {
                        await _accountService.ReverseIdentityChanges(oldIdentity);
                        return new JsonResult(new { Result = RESULTS.INTERRUPTED, Message = "The confirmation email was failed to send. Your changes was not applied. Please try again." });
                    }
                }

            if (!string.IsNullOrEmpty(newIdentity.PhoneNumber) && !newIdentity.PhoneConfirmed) {
                //Send SMS to confirm phone number
            }

            return new JsonResult(new { Result = RESULTS.SUCCESS, Message = newIdentity });
        }

        [HttpPost("update-security")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,0,1,0,0,0,0,0")]
        public async Task<JsonResult> UpdateAccountPassword(AccountSecurityVM security) {
            _logger.LogInformation("AccountController.UpdateAccountPassword - hidrogenianId=" + security.Id);
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AccountController),
                Action = nameof(UpdateAccountPassword),
                Briefing = "Update security data for an account having hidrogenianId = " + security.Id,
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var validation = await _reCaptchaService.IsHumanRegistration(security.CaptchaToken);
            if (!validation.Result) return new JsonResult(validation);

            var isPasswordCorrect = await _authService.VerifyAccountPasswordFor(security.Id, security.Password);
            if (!isPasswordCorrect.HasValue || !isPasswordCorrect.Value)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "Your current Password seems to be incorrect. Please enter correct password and try again." });

            var verification = security.VerifyPassword();
            if (verification.Count != 0) {
                var messages = security.GenerateErrorMessages(verification);
                return new JsonResult(new { Result = RESULTS.FAILED, Message = messages });
            }

            var (key, value) = _authService.GenerateHashedPasswordAndSalt(security.NewPassword);
            security.Password = null;
            security.NewPassword = key;
            security.PasswordConfirm = value;

            var result = await _accountService.UpdatePasswordForAccount(security);

            return !result.HasValue ? new JsonResult(new { Result = RESULTS.FAILED, Message = "Unable to update password due to account not found. Please login again and try." })
                                    : (!result.Value ? new JsonResult(new { Result = RESULTS.FAILED, Message = "An error occurred while attempting to update your password. Please try again." })
                                                     : new JsonResult(new { Result = RESULTS.SUCCESS }));
        }

        [HttpPost("enable-or-refresh-two-fa")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,0,1,0,0,0,0,0")]
        public async Task<JsonResult> EnableTwoFactorAuthentication(TwoFaVM twoFa) {
            _logger.LogInformation("AccountController.EnableTwoFactorAuthentication - hidrogenianId=" + twoFa.Id);
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AccountController),
                Action = nameof(EnableTwoFactorAuthentication),
                Briefing = "Enable 2FA for an account having hidrogenianId = " + twoFa.Id,
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var validation = await _reCaptchaService.IsHumanRegistration(twoFa.CaptchaToken);
            if (!validation.Result) return new JsonResult(validation);

            var secretKey = HelperProvider.GenerateRandomString(12);

            var saved = await _userService.SaveTwoFaSecretKeyFor(twoFa.Id, secretKey);
            if (!saved.HasValue || !saved.Value)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "Error occurred while attempting to setup Two-Factor Authentication at the moment. Please try again." });
            
            var identity = await _accountService.GetAccountIdentity(twoFa.Id);
            var tfa = new TwoFactorAuthenticator();
            
            var authenticator = tfa.GenerateSetupCode(
                HidroConstants.PROJECT_NAME, identity.Email,
                secretKey, false, 200
            );

            twoFa.QrImageUrl = authenticator.QrCodeSetupImageUrl;
            twoFa.ManualQrCode = authenticator.ManualEntryKey;

            return new JsonResult(new { Result = RESULTS.SUCCESS, Message = twoFa });
        }

        [HttpPost("disable-two-fa")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,0,1,0,0,0,0,0")]
        public async Task<JsonResult> DisableTwoFactorAuthentication(TwoFaVM twoFa) {
            _logger.LogInformation("AccountController.DisableTwoFactorAuthentication - hidrogenianId=" + twoFa.Id);
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AccountController),
                Action = nameof(DisableTwoFactorAuthentication),
                Briefing = "Disable 2FA for an account having hidrogenianId = " + twoFa.Id,
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var validation = await _reCaptchaService.IsHumanRegistration(twoFa.CaptchaToken);
            if (!validation.Result) return new JsonResult(validation);

            var updated = await _userService.RemoveTwoFaSecretKeyFor(twoFa.Id);

            return !updated.HasValue ? new JsonResult(new { Result = RESULTS.FAILED, Message = "Unable to find your account with the given data. Please login again and try." })
                                     : (!updated.Value ? new JsonResult(new { Result = RESULTS.FAILED, Message = "Error occurred while removing your Two-Factor Authentication data. Please try again." })
                                                       : new JsonResult(new { Result = RESULTS.SUCCESS }));
        }

        private List<int> VerifyIdentityData(AccountIdentityVM identity) {
            _logger.LogInformation("AccountController.VerifyIdentityData - Service runs internally.");
            
            _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AccountController),
                Action = "private " + nameof(VerifyIdentityData),
                Data = JsonConvert.SerializeObject(identity),
                Briefing = "Internally check identity data for errors.",
                Severity = LOGGING.INFORMATION.GetValue()
            });
            
            var errors = identity.VerifyEmail();
            errors.AddRange(identity.VerifyUserName());
            errors.AddRange(identity.VerifyPhoneNumber());

            return errors;
        }
    }
}