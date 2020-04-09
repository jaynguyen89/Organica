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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static HelperLibrary.HidroEnums;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("account")]
    public class AccountController {

        public readonly ILogger<AccountController> _logger;
        public readonly IAccountService _accountService;
        private readonly IAuthenticationService _authService;
        private readonly IHidrogenianService _userService;
        private readonly IHidroProfileService _profileService;
        private readonly IEmailSenderService _emailService;
        private readonly IGoogleReCaptchaService _reCaptchaService;

        private readonly string PROJECT_FOLDER = Path.GetDirectoryName(Directory.GetCurrentDirectory()) + @"/Hidrogen/";

        public AccountController(
            ILogger<AccountController> logger,
            IAccountService accountService,
            IAuthenticationService authService,
            IHidrogenianService userService,
            IHidroProfileService profileService,
            IEmailSenderService emailService,
            IGoogleReCaptchaService reCaptchaService
        ) {
            _logger = logger;
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

            var identity = await _accountService.GetAccountIdentity(hidrogenianId);

            return identity == null ? new JsonResult(new { Result = RESULTS.FAILED, Message = "Unable to find an account with the given data. Please reload page to try again." })
                                    : new JsonResult(new { Result = RESULTS.SUCCESS, Message = identity });
        }

        [HttpGet("get-two-fa")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,1,0,0,0,0,0,0")]
        public async Task<JsonResult> GetTwoFactorDataFor(TwoFaVM twoFa) {
            _logger.LogInformation("AccountController.GetTwoFactorDataFor - hidrogenianId=" + twoFa.Id);

            var secretKey = await _accountService.RetrieveTwoFaSecretKeyFor(twoFa.Id);
            if (secretKey == null) return new JsonResult(new { Result =RESULTS.FAILED, Message = "Error occurred while looking for your Two-Factor Authentication." });

            if (secretKey.Length == 0) new JsonResult(new { Result = RESULTS.SUCCESS });

            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            var authenticator = tfa.GenerateSetupCode(
                HidroConstants.PROJECT_NAME, twoFa.Email,
                secretKey, false, 200
            );

            twoFa.QrImageUrl = authenticator.QrCodeSetupImageUrl;
            twoFa.ManualQrCode = authenticator.ManualEntryKey;

            return new JsonResult(new { Result = RESULTS.SUCCESS });
        }

        [HttpGet("get-time-logs/{hidrogenianId}")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,1,0,0,0,0,0,0")]
        public async Task<JsonResult> GetTimeStampsFor(int hidrogenianId) {
            _logger.LogInformation("AccountController.GetAccountTimeStamps - hidrogenianId=" + hidrogenianId);

            var timeStamps = await _accountService.GetAccountTimeStamps(hidrogenianId);

            return timeStamps == null ? new JsonResult(new { Result = RESULTS.FAILED, Message = "Failed to retrieve account activity logs. Please reload page to try again." })
                                      : new JsonResult(new { Result = RESULTS.SUCCESS, Message = timeStamps });
        }

        [HttpPost("update-identity")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,0,1,0,0,0,0,0")]
        public async Task<JsonResult> UpdateAccountIdentity(AccountIdentityVM identity) {
            _logger.LogInformation("AccountController.UpdateAccountIdentity - hidrogenianId=" + identity.Id);

            var validation = await _reCaptchaService.IsHumanRegistration(identity.CaptchaToken);
            if (!validation.Result) return new JsonResult(validation);

            var verification = VerifyIdentityData(identity);
            if (verification.Count != 0) {
                var messages = identity.GenerateErrorMessages(verification);
                return new JsonResult(new { Result = RESULTS.FAILED, Message = messages });
            }

            var updatedIdentity = await _accountService.UpdateIdentityForHidrogenian(identity);

            if (!updatedIdentity.Key)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "Account not found with the given data. Please try again." });

            if (updatedIdentity.Value == null)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "An error occurred while attempting to update your account. Please try again." });

            var oldIdentity = updatedIdentity.Value.Value.Key;
            var newIdentity = updatedIdentity.Value.Value.Value;
            var hidrogenian = new HidrogenianVM {
                Id = newIdentity.Id,
                Token = _authService.GenerateRandomToken()
            };

            if (!newIdentity.EmailConfirmed)
                if (await _userService.SetAccountConfirmationToken(hidrogenian)) {
                    var profile = await _profileService.GetPublicProfileFor(newIdentity.Id);

                    string emailTemplate;
                    using (StreamReader reader = File.OpenText(PROJECT_FOLDER + @"HtmlTemplates/EmailChanged.html")) {
                        emailTemplate = reader.ReadToEnd();
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

            return new JsonResult(new { });
        }

        [HttpPost("update-security")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,0,1,0,0,0,0,0")]
        public async Task<JsonResult> UpdateAccountPassword(AccountSecurityVM security) {
            _logger.LogInformation("AccountController.UpdateAccountPassword - hidrogenianId=" + security.Id);

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

            var salted = _authService.GenerateHashedPasswordAndSalt(security.NewPassword);
            security.Password = null;
            security.NewPassword = salted.Key;
            security.PasswordConfirm = salted.Value;

            var result = await _accountService.UpdatePasswordForAccount(security);

            return !result.HasValue ? new JsonResult(new { Result = RESULTS.FAILED, Message = "Unable to update password due to account not found. Please login again and try." })
                                    : (!result.Value ? new JsonResult(new { Result = RESULTS.FAILED, Message = "An error occurred while attempting to update your password. Please try agian." })
                                                     : new JsonResult(new { Result = RESULTS.SUCCESS }));
        }

        [HttpPost("enable-two-fa")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,0,1,0,0,0,0,0")]
        public async Task<JsonResult> EnableTwoFactorAuthentication(TwoFaVM twoFa) {
            _logger.LogInformation("AccountController.EnableTwoFactorAuthentication - hidrogenianId=" + twoFa.Id);

            var validation = await _reCaptchaService.IsHumanRegistration(twoFa.CaptchaToken);
            if (!validation.Result) return new JsonResult(validation);

            var secretKey = HelperProvider.GenerateRandomString(12);
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();

            var saved = await _userService.SaveTwoFaSecretKeyFor(twoFa.Id, secretKey);
            if (!saved.HasValue || !saved.Value)
                return new JsonResult(new { Result = RESULTS.FAILED, Message = "Error occurred while attempting to setup Two-Factor Authentication at the moment. Please try agian." });

            var authenticator = tfa.GenerateSetupCode(
                HidroConstants.PROJECT_NAME, twoFa.Email,
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
            _logger.LogInformation("AccountController.EnableTwoFactorAuthentication - hidrogenianId=" + twoFa.Id);

            var validation = await _reCaptchaService.IsHumanRegistration(twoFa.CaptchaToken);
            if (!validation.Result) return new JsonResult(validation);

            var updated = await _userService.RemoveTwoFaSecretKeyFor(twoFa.Id);

            return !updated.HasValue ? new JsonResult(new { Result = RESULTS.FAILED, Message = "Unable to find your account with the given data. Please login again and try." })
                                     : (!updated.Value ? new JsonResult(new { Result = RESULTS.FAILED, Message = "Error occurred while removing your Two-Factor Authentication data. Please try again." })
                                                       : new JsonResult(new { Result = RESULTS.SUCCESS }));
        }

        private List<int> VerifyIdentityData(AccountIdentityVM identity) {
            _logger.LogInformation("AccountController.VerifyIdentityData - Service runs internally.");

            var errors = identity.VerifyEmail();
            errors.AddRange(identity.VerifyUserName());
            errors.AddRange(identity.VerifyPhoneNumber());

            return errors;
        }
    }
}