using Hidrogen.Attributes;
using Hidrogen.Services;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static HelperLibrary.HidroEnums;

namespace Hidrogen.Controllers {

    [ApiController]
    [Route("profile")]
    public class ProfileController {

        public readonly ILogger<ProfileController> _logger;
        public readonly IHidroProfileService _profileService;

        public ProfileController(
            ILogger<ProfileController> logger,
            IHidroProfileService profileService
        ) {
            _logger = logger;
            _profileService = profileService;
        }

        [HttpGet("get-public-profile/{hidrogenianId}")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,1,0,0,0,0,0,0")]
        public async Task<JsonResult> RetrievePublicProfile(int hidrogenianId) {
            _logger.LogInformation("ProfileController.RetrievePublicProfile - Service starts.");

            var profile = await _profileService.GetPublicProfileFor(hidrogenianId);

            return profile == null ? new JsonResult(new { Result = RESULTS.FAILED, Message = "Unable to retrieve profile information at the moment. Please try again." })
                                   : new JsonResult(new { Result = RESULTS.SUCCESS, Message = profile });
        }

        [HttpPut("update-avatar")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,0,1,0,0,0,0,0")]
        public async Task<JsonResult> UpdateProfileAvatar(HidroProfileVM profile) {
            _logger.LogInformation("ProfileController.UpdateProfileAvatar - Service starts.");

            var result = await _profileService.UpdateHidrogenianAvatar(profile);

            return !result.HasValue ? new JsonResult(new { Result = RESULTS.FAILED, Message = "No avatar found with the given profile data. Unable to update." }) : (
                result.Value ? new JsonResult(new { Result = RESULTS.SUCCESS })
                             : new JsonResult(new { Result = RESULTS.FAILED, Message = "Error happenned while updating your avatar. Please try again." })
            );
        }

        [HttpDelete("remove-avatar/{profileId}")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,0,0,0,1,0,0,0")]
        public async Task<JsonResult> RemoveProfileAvatar(int profileId) {
            _logger.LogInformation("ProfileController.RemoveProfileAvatar - Service starts.");

            var result = await _profileService.DeleteAvatarInformation(profileId);

            return !result.HasValue ? new JsonResult(new { Result = RESULTS.FAILED, Message = "No avatar found with the given profile data. Unable to remove." }) : (
                result.Value ? new JsonResult(new { Result = RESULTS.SUCCESS })
                             : new JsonResult(new { Result = RESULTS.FAILED, Message = "Error happenned while removing your avatar. Please try again." })
            );
        }

        [HttpPost("update-profile")]
        [HidroActionFilter("Customer")]
        [HidroAuthorize("0,0,1,0,0,0,0,0")]
        public async Task<JsonResult> UpdatePublicProfile(HidroProfileVM profile) {
            _logger.LogInformation("ProfileController.UpdatePublicProfile - Service starts.");

            var verification = VerifyProfileData(profile);
            if (verification.Count != 0) {
                var messages = profile.GenerateProfileDataErrorMessages(verification);
                return new JsonResult(new { Result = RESULTS.FAILED, Message = messages });
            }

            var result = await _profileService.UpdatePublicProfile(profile);

            return !result.HasValue ? new JsonResult(new { Result = RESULTS.FAILED, Message = "No profile found with the given data. Unable to update." }) : (
                result.Value ? new JsonResult(new { Result = RESULTS.SUCCESS })
                             : new JsonResult(new { Result = RESULTS.FAILED, Message = "Error happenned while updating your profile. Please try again." })
            );
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
            errors.AddRange(profile.VerifyIntroduciton());

            return errors;
        }
    }
}
