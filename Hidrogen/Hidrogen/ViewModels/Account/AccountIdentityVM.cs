using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HelperLibrary.Common;
using Hidrogen.Models;

namespace Hidrogen.ViewModels.Account {

    public class AccountIdentityVM {

        public int Id { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneConfirmed { get; set; }

        public string CaptchaToken { get; set; }

        public static implicit operator AccountIdentityVM(Hidrogenian account) {
            return new AccountIdentityVM {
                Id = account.Id,
                Email = account.Email,
                EmailConfirmed = account.EmailConfirmed,
                UserName = account.UserName,
                PhoneNumber = account.PhoneNumber,
                PhoneConfirmed = account.PhoneNumberConfirmed
            };
        }

        private static readonly List<string> INVALIDS = new List<string> {
            "--", "_@", "-@", ".-", "-.", "._", "_.", " ", "@_", "@-", "__", "..", "_-", "-_"
        };

        //Reprocess the Email by trimming and converting it to lowercase
        public List<int> VerifyEmail() {
            Email = Email.Trim().ToLower();

            if (string.IsNullOrEmpty(Email) || string.IsNullOrWhiteSpace(Email))
                return new List<int> { 0 };

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
                return new List<int> { 4 };

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

        public List<int> VerifyPhoneNumber() {
            var errors = new List<int>();
            if (string.IsNullOrEmpty(PhoneNumber))
                return errors;

            PhoneNumber = PhoneNumber.Trim().Replace(HidroConstants.DOUBLE_SPACE, HidroConstants.WHITE_SPACE);
            if (string.IsNullOrWhiteSpace(PhoneNumber)) {
                PhoneNumber = null;
                return errors;
            }

            var lenTest = new Regex(@".{10,20}");
            if (!lenTest.IsMatch(PhoneNumber))
                errors.Add(8);

            var pTest = new Regex(@"^[\d\s+()\-*#]*$");
            if (!pTest.IsMatch(PhoneNumber))
                errors.Add(9);

            return errors;
        }

        public List<string> GenerateErrorMessages(List<int> errors) {
            var messages = new List<string>();

            //For Email
            if (errors.Contains(0)) messages.Add("Email field is missing input. Please enter your email.");
            if (errors.Contains(1)) messages.Add("Email is either too short or long. Min 10, Max 50 characters.");
            if (errors.Contains(2) || errors.Contains(3)) messages.Add("The email you enter seems to be invalid.");

            //For UserName
            if (errors.Contains(4)) messages.Add("Username field is missing input. Please enter a username.");
            if (errors.Contains(5)) messages.Add("Username is either too short or long. Min 3, Max 20 characters.");
            if (errors.Contains(6) || errors.Contains(7)) messages.Add("The username you enter is not in the required format.");

            //For PhoneNumber
            if (errors.Contains(8)) messages.Add("Phone Number is either too short or long. Min 10, Max 20.");
            if (errors.Contains(9)) messages.Add("Phone Number only contains digits and these special characters: +()-*#");

            return messages;
        }
    }
}