using HelperLibrary;
using HelperLibrary.Common;
using Hidrogen.ViewModels;
using Newtonsoft.Json;
using System;
using static HelperLibrary.HidroEnums;

namespace Hidrogen.Models {

    public partial class HidroProfile {

        public AvatarInformationVM ProcessAvatarInfo() {
            return string.IsNullOrEmpty(AvatarInformation) ? null : JsonConvert.DeserializeObject<AvatarInformationVM>(AvatarInformation);
        }

        public ProfileBirth ProduceBirthValues() {
            return !DateOfBith.HasValue ? new ProfileBirth {
                FriendlyBirth = HidroConstants.NA,
                Birth = null
            } : new ProfileBirth {
                FriendlyBirth = DateOfBith.Value.ToString(DATE_FORMATS.FULL_DATE_FRIENDLY.GetValue()),
                Birth = DateOfBith.Value
            };
        }

        public GENDERS ProduceGenderEnum() {
            return !Gender.HasValue ? GENDERS.OTHER : (
                    Gender.Value ? GENDERS.MALE : GENDERS.FEMALE
                );
        }

        public static implicit operator HidroProfile(HidroProfileVM profile) {
            return new HidroProfile {
                FamilyName = profile.FamilyName,
                GivenName = profile.GivenName,
                Gender = profile.Gender == GENDERS.OTHER ? null : (bool?)(profile.Gender == GENDERS.MALE),
                DateOfBith = profile.Birthday.Birth,
                Ethnicity = profile.Ethnicity,
                Company = profile.Company,
                JobTitle = profile.JobTitle,
                PersonalWebsite = profile.Website,
                SelfIntroduction = profile.SelfIntroduction
            };
        }
    }
}
