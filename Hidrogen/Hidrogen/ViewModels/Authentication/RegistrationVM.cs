using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HelperLibrary;
using HelperLibrary.Common;

namespace Hidrogen.ViewModels.Authentication {

    public class RegistrationVM {

        public string UserName { get; set; }

        public string Email { get; set; } //also for resetting password

        public string TempPassword { get; set; } //only for resetting password

        public string Password { get; set; } = null; //also for resetting password

        public string PasswordConfirm { get; set; } //also for resetting password

        public string FamilyName { get; set; }

        public string GivenName { get; set; }

        public string RecoveryToken { get; set; } //also for resetting password

        public string CaptchaToken { get; set; } //also for resetting password

        private static readonly List<string> INVALIDS = new List<string>() {
            "--", "_@", "-@", ".-", "-.", "._", "_.", " ", "@_", "@-", "__", "..", "_-", "-_"
        };

        //Reprocess the Email by trimming and converting it to lowercase
        public List<int> VerifyEmail() {
            Email = Email.Trim().ToLower();

            if (string.IsNullOrEmpty(Email) || string.IsNullOrWhiteSpace(Email))
                return new List<int>() { 0 };

            var errors = new List<int>();

            var lenTest = new Regex(@".{10,50}");
            if (!lenTest.IsMatch(Email))
                errors.Add(1);

            var fmTest = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!fmTest.IsMatch(Email) || INVALIDS.Any(Email.Contains))
                errors.Add(2);

            if (errors.Count == 0) {
                var domain = Email.Split("@")[1];
                if (domain.Split(".").Length > 3)
                    errors.Add(3);
            }

            return errors;
        }

        //Only "trim" whitespaces in the UserName, keep the resulted string as is
        public List<int> VerifyUserName() {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrWhiteSpace(UserName))
                return new List<int>() { 4 };

            UserName = UserName.Trim();
            var errors = new List<int>();

            var lenTest = new Regex(@".{3,20}");
            if (!lenTest.IsMatch(UserName))
                errors.Add(5);

            var spTest = new Regex(@"^[0-9A-Za-z_\-.]*$");
            if (!spTest.IsMatch(UserName))
                errors.Add(6);

            if (errors.Count == 0 && (INVALIDS.Any(UserName.Contains) ||
                "_-.".Contains(UserName[0]) || "_-.".Contains(UserName[^1])))
                errors.Add(7);

            return errors;
        }

        public List<int> VerifyPassword(string password = null) {
            if (string.IsNullOrEmpty(Password) || string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrEmpty(PasswordConfirm) || string.IsNullOrWhiteSpace(PasswordConfirm))
                return new List<int>() { 8 };

            var errors = new List<int>();

            if (Password != PasswordConfirm)
                errors.Add(9);

            var lenTest = new Regex(@".{6,20}");
            if (!lenTest.IsMatch(Password))
                errors.Add(10);

            var lowTest = new Regex(@"[a-z]+");
            if (!lowTest.IsMatch(Password))
                errors.Add(11);

            var capTest = new Regex(@"[A-Z]+");
            if (!capTest.IsMatch(Password))
                errors.Add(12);

            var digitTest = new Regex(@"[\d]+");
            if (!digitTest.IsMatch(Password))
                errors.Add(13);

            if (!errors.Contains(10) && Password.Contains(" "))
                errors.Add(14);

            var spTest = new Regex(@"[!@#$%^&*_+\.]+");
            if (!spTest.IsMatch(Password))
                errors.Add(15);

            return errors;
        }

        //Reprocess the FamilyName before checking
        public List<int> VerifyFamilyName() {
            if (string.IsNullOrEmpty(FamilyName) || string.IsNullOrWhiteSpace(FamilyName))
                return new List<int>() { 16 };

            FamilyName = HelperProviders.CapitalizeFirstLetterOfEachWord(FamilyName.Trim());
            var errors = new List<int>();

            var lenTest = new Regex(@".{1,30}");
            if (!lenTest.IsMatch(FamilyName))
                errors.Add(17);

            var spTest = new Regex(@"^[A-Za-z_\-.'() ]*$");
            if (!spTest.IsMatch(FamilyName))
                errors.Add(18);

            return errors;
        }

        //Reprocess the GivenName before checking
        public List<int> VerifyGivenName() {
            if (string.IsNullOrEmpty(GivenName) || string.IsNullOrWhiteSpace(GivenName))
                return new List<int>() { 19 };

            GivenName = HelperProviders.CapitalizeFirstLetterOfEachWord(GivenName.Trim());
            var errors = new List<int>();

            var lenTest = new Regex(@".{1,50}");
            if (!lenTest.IsMatch(GivenName))
                errors.Add(20);

            var spTest = new Regex(@"^[A-Za-z_\-.'() ]*$");
            if (!spTest.IsMatch(GivenName))
                errors.Add(21);

            return errors;
        }

        public List<string> GenerateErrorMessages(List<int> errors) {
            var messages = new List<string>();

            //For Email
            if (errors.Contains(0)) messages.Add("Email field is missing input. Email is required for registration.");
            if (errors.Contains(1)) messages.Add("Email is either too short or long. Min 10, Max 50 characters.");
            if (errors.Contains(2) || errors.Contains(3)) messages.Add("The email you enter seems to be invalid.");

            //For UserName
            if (errors.Contains(4)) messages.Add("Username field is missing input. Username is required for registration.");
            if (errors.Contains(5)) messages.Add("Username is either too short or long. Min 3, Max 20 characters.");
            if (errors.Contains(6) || errors.Contains(7)) messages.Add("The username you enter is not in the required format.");

            //For Password
            if (errors.Contains(8)) messages.Add("Password and/or Confirm Password fields are missing inputs.");
            if (errors.Contains(9)) messages.Add("Password and Confirm Password do not match.");
            if (errors.Contains(10)) messages.Add("Password should be 6 to 20 characters in length.");
            if (errors.Contains(11)) messages.Add("Password must contain at least 1 lowercase character.");
            if (errors.Contains(12)) messages.Add("Password must contain at least 1 uppercase character.");
            if (errors.Contains(13)) messages.Add("Password must contain at least 1 digit.");
            if (errors.Contains(14)) messages.Add("Password must NOT contain white-space.");
            if (errors.Contains(15)) messages.Add("Password must contain at least 1 of these special characters: !@#$%^&*_+.");

            //For FamilyName
            if (errors.Contains(16)) messages.Add("Family Name field is missing input. Please enter your Family Name.");
            if (errors.Contains(17)) messages.Add("Family Name is too long. Max 50 characters.");
            if (errors.Contains(18)) messages.Add("Family Name can only contain these special characters: _-.(')");

            //For GivenName
            if (errors.Contains(19)) messages.Add("Given Name field is missing input. Please enter your Given Name.");
            if (errors.Contains(20)) messages.Add("Given Name is too long. Max 50 characters.");
            if (errors.Contains(21)) messages.Add("Given Name can only contain these special characters: _-.(')");

            return messages;
        }
    }
}
