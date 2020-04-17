using HelperLibrary;
using HelperLibrary.Common;
using Hidrogen.ViewModels;
using Newtonsoft.Json;
using static HelperLibrary.HidroEnums;

namespace Hidrogen.Models {

    public partial class HidroProfile {

        public AvatarVM ProcessAvatarInfo() {
            return string.IsNullOrEmpty(AvatarInformation) ? null : JsonConvert.DeserializeObject<AvatarVM>(AvatarInformation);
        }

        public ProfileBirth ProduceBirthValues() {
            return !DateOfBirth.HasValue ? new ProfileBirth {
                FriendlyBirth = HidroConstants.NA,
                Birth = null
            } : new ProfileBirth {
                FriendlyBirth = DateOfBirth.Value.ToString(DATE_FORMATS.FULL_DATE_FRIENDLY.GetValue()),
                Birth = DateOfBirth.Value
            };
        }

        public static implicit operator HidroProfile(HidroProfileVM profile) {
            return new HidroProfile {
                HidrogenianId = profile.HidrogenianId,
                FamilyName = profile.FamilyName,
                GivenName = profile.GivenName,
                Gender = profile.Gender == 0 ? (bool?)null : (profile.Gender == 1),
                DateOfBirth = profile.Birthday.Birth,
                Ethnicity = profile.Ethnicity,
                Company = profile.Company,
                JobTitle = profile.JobTitle,
                PersonalWebsite = profile.Website,
                SelfIntroduction = profile.SelfIntroduction
            };
        }
    }
}
