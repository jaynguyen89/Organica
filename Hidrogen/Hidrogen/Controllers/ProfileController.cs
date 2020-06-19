using System.Collections.Generic;
using System.Threading.Tasks;
using HelperLibrary;
using Hidrogen.Attributes;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels;
using MethaneLibrary.Interfaces;
using MethaneLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WaterLibrary.Interfaces;
using WaterLibrary.ViewModels;
using static HelperLibrary.HidroEnums;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("profile")]
    public class ProfileController : AppController {
        
        private readonly ILogger<ProfileController> _logger;
        private readonly IRuntimeLogService _runtimeLogger;
        private readonly IWaterService _waterService;
        private readonly IHidroProfileService _profileService;

        public ProfileController(
            ILogger<ProfileController> logger,
            IRuntimeLogService runtimeLogger,
            IWaterService waterService,
            IHidroProfileService profileService,
            IDistributedCache redisCache
        ) : base(redisCache) {
            _logger = logger;
            _runtimeLogger = runtimeLogger;
            _waterService = waterService;
            _profileService = profileService;
        }

        [HttpPost("save-avatar")]
        [HidroActionFilter(ROLES.CUSTOMER)]
        [HidroAuthorize(PERMISSIONS.CREATE)]
        public async Task<JsonResult> SaveProfileAvatar([FromForm] AssetFormVM uploading) {
            _logger.LogInformation("ProfileController.SaveProfileAvatar - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(ProfileController),
                Action = nameof(SaveProfileAvatar),
                Briefing = "Save the uploaded photo as profile avatar for user having hidrogenianId = " + uploading.HidrogenianId,
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var errors = uploading.CheckFile();
            if (errors.Count != 0) {
                var messages = uploading.GenerateErrorMessages(errors);
                return new JsonResult(new { Result = RESULTS.FAILED, Message = messages });
            }

            var response = await _waterService.SendSaveAvatarRequestToWater(uploading);
            if (response == null) return new JsonResult(new { Result = RESULTS.FAILED, Message = "An error occurred while we're processing your request. Please try again." });
            if (response.Error) return new JsonResult(new { Result = RESULTS.FAILED, response.ErrorMessage });

            return await UpdateHidrogenianAvatarInternally(uploading.HidrogenianId, uploading.ApiKey, response.Result);
        }

        [HttpGet("get-private-profile/{hidrogenianId}")]
        [HidroActionFilter(ROLES.CUSTOMER)]
        [HidroAuthorize(PERMISSIONS.VIEW)]
        public async Task<JsonResult> RetrievePrivateProfile(int hidrogenianId) {
            _logger.LogInformation("ProfileController.RetrievePublicProfile - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(ProfileController),
                Action = nameof(RetrievePrivateProfile),
                Briefing = "Get private profile details for user having hidrogenianId = " + hidrogenianId,
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var profile = await ReadFromRedisCacheAsync<HidroProfileVM>("Profile_PrivateProfile");
            if (profile != null) return new JsonResult(new { Result = RESULTS.SUCCESS, Message = profile });

            profile = await _profileService.GetPublicProfileFor(hidrogenianId);
            if (profile == null) return new JsonResult(new {Result = RESULTS.FAILED, Message = "Unable to retrieve profile information at the moment. Please try again."});

            await InsertRedisCacheEntryAsync("Profile_PrivateProfile", profile);
            return new JsonResult(new { Result = RESULTS.SUCCESS, Message = profile });
        }

        [HttpPut("update-avatar")]
        [HidroActionFilter(ROLES.CUSTOMER)]
        [HidroAuthorize(PERMISSIONS.EDIT_OWN)]
        public async Task<JsonResult> UpdateProfileAvatar([FromForm] AssetReplaceVM uploading) {
            _logger.LogInformation("ProfileController.UpdateProfileAvatar - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(ProfileController),
                Action = nameof(UpdateProfileAvatar),
                Briefing = "Save new photo to update user profile with hidrogenianId = " + uploading.HidrogenianId,
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var errors = uploading.CheckFile();
            if (errors.Count != 0) {
                var messages = uploading.GenerateErrorMessages(errors);
                return new JsonResult(new { Result = RESULTS.FAILED, Message = messages });
            }

            var response = await _waterService.SendReplaceAvatarRequestToWater(uploading);
            if (response == null) return new JsonResult(new { Result = RESULTS.FAILED, Message = "An error occurred while we're processing your request. Please try again." });
            if (response.Error) return new JsonResult(new { Result = RESULTS.FAILED, Message = response.ErrorMessage});

            return await UpdateHidrogenianAvatarInternally(uploading.HidrogenianId, uploading.ApiKey, response.Result);
        }

        [HttpDelete("remove-avatar/{hidrogenianId}/{apikey}")]
        [HidroActionFilter(ROLES.CUSTOMER)]
        [HidroAuthorize(PERMISSIONS.DELETE_OWN)]
        public async Task<JsonResult> RemoveProfileAvatar(int hidrogenianId, string apikey) {
            _logger.LogInformation("ProfileController.RemoveProfileAvatar - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(ProfileController),
                Action = nameof(RemoveProfileAvatar),
                Briefing = "Remove user avatar photo with hidrogenianId = " + hidrogenianId,
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var result = await _profileService.DeleteAvatarInformation(hidrogenianId);
            if (result == null) return new JsonResult(new { Result = RESULTS.FAILED, Message = "No avatar found with the given profile data. Unable to remove." });
            if (result.Length == 0) return new JsonResult(new { Result = RESULTS.FAILED, Message = "Error happened while removing your avatar. Please try again." });

            await RemoveRedisCacheEntryAsync("Profile_AvatarInfo");
            
            var avatar = JsonConvert.DeserializeObject<AvatarVM>(result);
            var deleted = await _waterService.SendDeleteAvatarRequestToWater(apikey, avatar.Name);

            if (deleted == null) return new JsonResult(new { Result = RESULTS.INTERRUPTED, Message = "Your avatar has been removed." });

            return deleted.Error ? new JsonResult(new { Result = RESULTS.INTERRUPTED, Message = "Your avatar was removed." })
                                 : new JsonResult(new { Result = RESULTS.SUCCESS, Message = "Your avatar has been removed successfully." });
        }

        [HttpPost("update-private-profile")]
        [HidroActionFilter(ROLES.CUSTOMER)]
        [HidroAuthorize(PERMISSIONS.EDIT_OWN)]
        public async Task<JsonResult> UpdatePrivateProfile(HidroProfileVM profile) {
            _logger.LogInformation("ProfileController.UpdatePrivateProfile - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(ProfileController),
                Action = nameof(RemoveProfileAvatar),
                Data = JsonConvert.SerializeObject(profile),
                Briefing = "Update private profile in database for a user.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var verification = VerifyProfileData(profile);
            if (verification.Count != 0) {
                var messages = profile.GenerateProfileDataErrorMessages(verification);
                return new JsonResult(new { Result = RESULTS.FAILED, Message = messages });
            }

            var result = await _profileService.UpdatePrivateProfile(profile);
            if (!result.HasValue) return new JsonResult(new {Result = RESULTS.FAILED, Message = "No profile found with the given data. Unable to update."});
            if (!result.Value) return new JsonResult(new {Result = RESULTS.FAILED, Message = "Error happened while updating your profile. Please try again."});

            await RemoveRedisCacheEntryAsync("Profile_PrivateProfile");
            return new JsonResult(new {Result = RESULTS.SUCCESS});
        }

        private async Task<JsonResult> UpdateHidrogenianAvatarInternally(int hidrogenianId, string apiKey, ResultVM avatarResult) {
            _logger.LogInformation("ProfileController.UpdateHidrogenianAvatarInternally - Service runs internally.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(ProfileController),
                Action = "private " + nameof(UpdateHidrogenianAvatarInternally),
                Briefing = "Internally save new avatar information to profile for user.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var avatar = (AvatarVM)avatarResult;
            var profile = new HidroProfileVM {
                HidrogenianId = hidrogenianId,
                Avatar = avatar
            };

            var result = await _profileService.UpdateHidrogenianAvatar(profile);
            if (result.HasValue && result.Value) {
                await RemoveRedisCacheEntryAsync("Profile_PrivateProfile");
                return new JsonResult(new {Result = RESULTS.SUCCESS, Message = avatar.Name});
            }
            
            var deleted = await _waterService.SendDeleteAvatarRequestToWater(apiKey, avatar.Name);
            if (deleted == null) return new JsonResult(new { Result = RESULTS.INTERRUPTED, Message = "Your avatar was uploaded, however, an error occurred while we update your profile. Please reload page and try again." });

            return !deleted.Error ? new JsonResult(new { Result = RESULTS.INTERRUPTED, Message = "Your avatar was uploaded, however, an error occurred while we update your profile. Changes have been reverted. Please try again." })
                                  : new JsonResult(new { Result = RESULTS.INTERRUPTED, Message = "Your avatar was uploaded, however, an error occurred while we update your profile. Please try again" });

        }

        private List<int> VerifyProfileData(HidroProfileVM profile) {
            _logger.LogInformation("ProfileController.VerifyProfileData - Verification starts.");
            _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(ProfileController),
                Action = "private " + nameof(VerifyProfileData),
                Briefing = "Internally check the submitted profile data for any errors.",
                Severity = LOGGING.INFORMATION.GetValue()
            });

            var errors = profile.VerifyFamilyName();
            errors.AddRange(profile.VerifyGivenName());
            errors.AddRange(profile.VerifyBirthday());
            errors.AddRange(profile.VerifyCompany());
            errors.AddRange(profile.VerifyEthnicity());
            errors.AddRange(profile.VerifyJobTitle());
            errors.AddRange(profile.VerifyWebsite());
            errors.AddRange(profile.VerifyIntroduction());

            return errors;
        }
    }
}
