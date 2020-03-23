using static HelperLibrary.HidroEnums;
using HelperLibrary.Common;
using System;
using Hidrogen.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using HelperLibrary;
using System.Text.RegularExpressions;

namespace Hidrogen.ViewModels {

    public class HidroProfileVM {

        public int Id { get; set; } //Use for updating/removing profile/avatar

        public int HidrogenianId { get; set; }

        public string FamilyName { get; set; }

        public string GivenName { get; set; }

        public string FullName => GivenName + HidroConstants.WHITE_SPACE + FamilyName;

        public string Avatar { get; set; } //Use for updating/removing avatar

        public ProfileBirth Birthday { get; set; }

        public GENDERS Gender { get; set; }

        public string Ethnicity { get; set; }

        public string Company { get; set; }

        public string JobTitle { get; set; }

        public string Website { get; set; }

        public string SelfIntroduction { get; set; }

        public static implicit operator HidroProfileVM(HidroProfile profile) {
            var avatar = profile.ProcessAvatarInfo();

            return new HidroProfileVM {
                Id = profile.Id,
                HidrogenianId = profile.HidrogenianId,
                FamilyName = profile.FamilyName,
                GivenName = profile.GivenName,
                Avatar = avatar.Medium?.FileUrl,
                Birthday = profile.ProduceBirthValues(),
                Gender = profile.ProduceGenderEnum(),
                Ethnicity = profile.Ethnicity,
                Company = profile.Company,
                JobTitle = profile.JobTitle,
                Website = profile.PersonalWebsite,
                SelfIntroduction = profile.SelfIntroduction
            };
        }

        public List<int> VerifyBirthday() {
            var errors = new List<int>();

            if (!Birthday.Birth.HasValue)
                return errors;

            if (Birthday.Birth.Value.Date >= DateTime.UtcNow.Date)
                errors.Add(0);

            if (Birthday.Birth.Value.Date < DateTime.UtcNow.Date.AddYears(-100))
                errors.Add(1);

            return errors;
        }

        //Remove double-spaces and reprocess the value
        public List<int> VerifyEthnicity() {
            var errors = new List<int>();
            if (Ethnicity == null) return errors;

            Ethnicity = Ethnicity.Trim().Replace(HidroConstants.DOUBLE_SPACE, HidroConstants.WHITE_SPACE);
            if (string.IsNullOrEmpty(Ethnicity) || string.IsNullOrWhiteSpace(Ethnicity)) {
                errors.Add(2);
                return errors;
            }

            Ethnicity = HelperProviders.CapitalizeFirstLetterOfEachWord(Ethnicity);

            var lenTest = new Regex(@".{1,30}");
            if (!lenTest.IsMatch(Ethnicity))
                errors.Add(3);

            var enTest = new Regex(@"^[A-Za-z\-.' ]*$");
            if (!enTest.IsMatch(Ethnicity))
                errors.Add(4);

            return errors;
        }

        //Remove double-spaces and reprocess the value
        public List<int> VerifyCompany() {
            var errors = new List<int>();
            if (Company == null) return errors;

            Company = Company.Trim().Replace(HidroConstants.DOUBLE_SPACE, HidroConstants.WHITE_SPACE);
            if (string.IsNullOrEmpty(Company) || string.IsNullOrWhiteSpace(Company)) {
                errors.Add(5);
                return errors;
            }

            Company = HelperProviders.CapitalizeFirstLetterOfEachWord(Company);

            var lenTest = new Regex(@".{1,30}");
            if (!lenTest.IsMatch(Company))
                errors.Add(6);

            var cpTest = new Regex(@"^[A-Za-z&,\-.'() ]*$");
            if (!cpTest.IsMatch(Company))
                errors.Add(7);

            return errors;
        }

        //Remove double-spaces and reprocess the value
        public List<int> VerifyJobTitle() {
            var errors = new List<int>();
            if (JobTitle == null) return errors;

            JobTitle = JobTitle.Trim().Replace(HidroConstants.DOUBLE_SPACE, HidroConstants.WHITE_SPACE);
            if (string.IsNullOrEmpty(JobTitle) || string.IsNullOrWhiteSpace(JobTitle)) {
                errors.Add(8);
                return errors;
            }

            JobTitle = HelperProviders.CapitalizeFirstLetterOfEachWord(JobTitle);

            var lenTest = new Regex(@".{1,30}");
            if (!lenTest.IsMatch(JobTitle))
                errors.Add(9);

            var jTest = new Regex(@"^[A-Za-z,\-.'() ]*$");
            if (!jTest.IsMatch(JobTitle))
                errors.Add(10);

            return errors;
        }

        //Remove all spaces in the URL
        public List<int> VerifyWebsite() {
            var errors = new List<int>();
            if (Website == null) return errors;

            Website = Website.ToLower().Trim().Replace(HidroConstants.WHITE_SPACE, string.Empty);
            if (string.IsNullOrEmpty(Company)) {
                errors.Add(11);
                return errors;
            }

            var lenTest = new Regex(@".{10,100}");
            if (!lenTest.IsMatch(Website))
                errors.Add(12);

            var webTest = new Regex(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$");
            if (!webTest.IsMatch(Website))
                errors.Add(13);

            return errors;
        }

        public List<int> VerifyIntroduciton() {
            var errors = new List<int>();
            if (SelfIntroduction == null) return errors;

            var lenTest = new Regex(@".{30,500}");
            if (!lenTest.IsMatch(SelfIntroduction))
                errors.Add(14);

            return errors;
        }

        //Reprocess the FamilyName before checking
        public List<int> VerifyFamilyName() {
            if (string.IsNullOrEmpty(FamilyName) || string.IsNullOrWhiteSpace(FamilyName))
                return new List<int>() { 15 };

            FamilyName = HelperProviders.CapitalizeFirstLetterOfEachWord(FamilyName.Trim());
            var errors = new List<int>();

            var lenTest = new Regex(@".{1,30}");
            if (!lenTest.IsMatch(FamilyName))
                errors.Add(16);

            var spTest = new Regex(@"^[A-Za-z_\-.'() ]*$");
            if (!spTest.IsMatch(FamilyName))
                errors.Add(17);

            return errors;
        }

        //Reprocess the GivenName before checking
        public List<int> VerifyGivenName() {
            if (string.IsNullOrEmpty(GivenName) || string.IsNullOrWhiteSpace(GivenName))
                return new List<int>() { 18 };

            GivenName = HelperProviders.CapitalizeFirstLetterOfEachWord(GivenName.Trim());
            var errors = new List<int>();

            var lenTest = new Regex(@".{1,50}");
            if (!lenTest.IsMatch(GivenName))
                errors.Add(19);

            var spTest = new Regex(@"^[A-Za-z_\-.'() ]*$");
            if (!spTest.IsMatch(GivenName))
                errors.Add(20);

            return errors;
        }

        internal object GenerateProfileDataErrorMessages(List<int> errors) {
            var messages = new List<string>();

            //For Birthday
            if (errors.Contains(0)) messages.Add("Your birthday cannot be after today.");
            if (errors.Contains(1)) messages.Add("Really? You're sure your age is over 100 years old? Please set valid birthday.");

            //For Ethnicity
            if (errors.Contains(2)) messages.Add("Ethnicity field is a sequence of white-spaces. Please leave it empty if you don't want to set an enthnicity.");
            if (errors.Contains(3)) messages.Add("Ethnicity is too long. Max 30 characters.");
            if (errors.Contains(4)) messages.Add("Ethnicity can only contain these special characters: -.'");

            //For Company
            if (errors.Contains(5)) messages.Add("Company field is a sequence of white-spaces. Please leave it empty if you don't want to set a company.");
            if (errors.Contains(6)) messages.Add("Company is either too short or long. Max 30 characters.");
            if (errors.Contains(7)) messages.Add("Company can only contain these special characters: &,-.'()");

            //For JobTitle
            if (errors.Contains(8)) messages.Add("Job Title field is a sequence of white-spcaes. Please leave it empty if you don't want to set a job title.");
            if (errors.Contains(9)) messages.Add("Job Title is too long. Max 30 characters.");
            if (errors.Contains(10)) messages.Add("Job Title can only contain these special characters: ,-.'()");

            //For Website
            if (errors.Contains(11)) messages.Add("Website field is a sequence of white-spaces. Please leave it empty if you don't want to set a website.");
            if (errors.Contains(12)) messages.Add("Website URL is either too short or long. Min 10, Max 100 characters. ");
            if (errors.Contains(13)) messages.Add("Website URL seems to be invalid. Please enter a valid URL.");

            //For Introduction
            if (errors.Contains(14)) messages.Add("Your introduction is either too short or long. Min 30, Max 500 characters.");

            //For FamilyName
            if (errors.Contains(15)) messages.Add("Family Name field is a sequence of white-spaces. Please leave it empty if you don't wnat to provide your name.");
            if (errors.Contains(16)) messages.Add("Family Name is either too short or long.");
            if (errors.Contains(17)) messages.Add("Family Name can only contain these special characters: _-.(')");

            //For GivenName
            if (errors.Contains(18)) messages.Add("Given Name field is a sequence of white-spaces. Please leave it empty if you don't wnat to provide your name.");
            if (errors.Contains(19)) messages.Add("Given Name is either too short or long.");
            if (errors.Contains(20)) messages.Add("Given Name can only contain these special characters: _-.(')");

            return messages;
        }
    }

    public class ProfileBirth {
        
        public string FriendlyBirth { get; set; }

        public DateTime? Birth { get; set; }
    }
}