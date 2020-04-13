using System.Collections.Generic;
using System.Threading.Tasks;
using Hidrogen.Attributes;
using Hidrogen.Services;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WaterLibrary.Interfaces;
using WaterLibrary.ViewModels;
using static HelperLibrary.HidroEnums;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("profile")]
    public class ProfileController {
        
        private readonly ILogger<ProfileController> _logger;
        private readonly IWaterService _waterService;
        private readonly IHidroProfileService _profileService;

        public ProfileController(
            ILogger<ProfileController> logger,
            IWaterService waterService,
            IHidroProfileService profileService
        ) {
            _logger = logger;
            _waterService = waterService;
            _profileService = profileService;
        }

        [HttpPost("save-avatar")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("1,0,0,0,0,0,0,0")]
        public async Task<JsonResult> SaveProfileAvatar([FromForm] AssetFormVM uploading) {
            _logger.LogInformation("ProfileController.SaveProfileAvatar - Service starts.");

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
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,1,0,0,0,0,0,0")]
        public async Task<JsonResult> RetrievePrivateProfile(int hidrogenianId) {
            _logger.LogInformation("ProfileController.RetrievePublicProfile - Service starts.");

            var profile = await _profileService.GetPublicProfileFor(hidrogenianId);

            return profile == null ? new JsonResult(new { Result = RESULTS.FAILED, Message = "Unable to retrieve profile information at the moment. Please try again." })
                                   : new JsonResult(new { Result = RESULTS.SUCCESS, Message = profile });
        }

        [HttpPut("update-avatar")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,0,1,0,0,0,0,0")]
        public async Task<JsonResult> UpdateProfileAvatar([FromForm] AssetReplaceVM uploading) {
            _logger.LogInformation("ProfileController.UpdateProfileAvatar - Service starts.");

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
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,0,0,0,1,0,0,0")]
        public async Task<JsonResult> RemoveProfileAvatar(int hidrogenianId, string apikey) {
            _logger.LogInformation("ProfileController.RemoveProfileAvatar - Service starts.");

            var result = await _profileService.DeleteAvatarInformation(hidrogenianId);
            if (result == null) return new JsonResult(new { Result = RESULTS.FAILED, Message = "No avatar found with the given profile data. Unable to remove." });
            if (result.Length == 0) return new JsonResult(new { Result = RESULTS.FAILED, Message = "Error happened while removing your avatar. Please try again." });

            var avatar = JsonConvert.DeserializeObject<AvatarVM>(result);
            var deleted = await _waterService.SendDeleteAvatarRequestToWater(apikey, avatar.Name);

            if (deleted == null) return new JsonResult(new { Result = RESULTS.INTERRUPTED, Message = "Your avatar has been removed." });

            return deleted.Error ? new JsonResult(new { Result = RESULTS.INTERRUPTED, Message = "Your avatar was removed." })
                                 : new JsonResult(new { Result = RESULTS.SUCCESS, Message = "Your avatar has been removed successfully." });
        }

        [HttpPost("update-private-profile")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,0,1,0,0,0,0,0")]
        public async Task<JsonResult> UpdatePrivateProfile(HidroProfileVM profile) {
            _logger.LogInformation("ProfileController.UpdatePrivateProfile - Service starts.");

            var verification = VerifyProfileData(profile);
            if (verification.Count != 0) {
                var messages = profile.GenerateProfileDataErrorMessages(verification);
                return new JsonResult(new { Result = RESULTS.FAILED, Message = messages });
            }

            var result = await _profileService.UpdatePrivateProfile(profile);

            return !result.HasValue ? new JsonResult(new { Result = RESULTS.FAILED, Message = "No profile found with the given data. Unable to update." }) : (
                result.Value ? new JsonResult(new { Result = RESULTS.SUCCESS })
                             : new JsonResult(new { Result = RESULTS.FAILED, Message = "Error happened while updating your profile. Please try again." })
            );
        }

        private async Task<JsonResult> UpdateHidrogenianAvatarInternally(int hidrogenianId, string apiKey, ResultVM avatarResult) {
            _logger.LogInformation("ProfileController.UpdateHidrogenianAvatarInternally - Service runs internally.");

            var avatar = (AvatarVM)avatarResult;
            var profile = new HidroProfileVM {
                HidrogenianId = hidrogenianId,
                Avatar = avatar
            };

            var result = await _profileService.UpdateHidrogenianAvatar(profile);
            if (!result.HasValue || !result.Value) {
                var deleted = await _waterService.SendDeleteAvatarRequestToWater(apiKey, avatar.Name);

                if (deleted == null) return new JsonResult(new { Result = RESULTS.INTERRUPTED, Message = "Your avatar was uploaded, however, an error occurred while we update your profile. Please reload page and try again." });

                return !deleted.Error ? new JsonResult(new { Result = RESULTS.INTERRUPTED, Message = "Your avatar was uploaded, however, an error occurred while we update your profile. Changes have been reverted. Please try again." })
                                      : new JsonResult(new { Result = RESULTS.INTERRUPTED, Message = "Your avatar was uploaded, however, an error occurred while we update your profile. Please try again" });
            }

            return new JsonResult(new { Result = RESULTS.SUCCESS, Message = avatar.Name });
        }

        private List<int> VerifyProfileData(HidroProfileVM profile) {
            _logger.LogInformation("ProfileController.VerifyProfileData - Verification starts.");

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
