using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Hidrogen.ViewModels.Account {

    public class AccountSecurityVM {

        public int Id { get; set; }

        public string Password { get; set; }

        public string NewPassword { get; set; }

        public string PasswordConfirm { get; set; }

        public string CaptchaToken { get; set; }

        public List<int> VerifyPassword(string password = null) {
            if (string.IsNullOrEmpty(NewPassword) || string.IsNullOrWhiteSpace(NewPassword) ||
                string.IsNullOrEmpty(PasswordConfirm) || string.IsNullOrWhiteSpace(PasswordConfirm))
                return new List<int> { 0 };

            var errors = new List<int>();

            if (NewPassword != PasswordConfirm)
                errors.Add(1);

            var lenTest = new Regex(@".{6,20}");
            if (!lenTest.IsMatch(NewPassword))
                errors.Add(2);

            var lowTest = new Regex(@"[a-z]+");
            if (!lowTest.IsMatch(NewPassword))
                errors.Add(3);

            var capTest = new Regex(@"[A-Z]+");
            if (!capTest.IsMatch(NewPassword))
                errors.Add(4);

            var digitTest = new Regex(@"[\d]+");
            if (!digitTest.IsMatch(NewPassword))
                errors.Add(5);

            if (!errors.Contains(10) && NewPassword.Contains(" "))
                errors.Add(6);

            var spTest = new Regex(@"[!@#$%^&*_+\.]+");
            if (!spTest.IsMatch(NewPassword))
                errors.Add(7);

            return errors;
        }

        public List<string> GenerateErrorMessages(List<int> errors) {
            var messages = new List<string>();

            if (errors.Contains(0)) messages.Add("Password and/or Confirm Password fields are missing inputs.");
            if (errors.Contains(1)) messages.Add("Password and Confirm Password do not match.");
            if (errors.Contains(2)) messages.Add("Password should be 6 to 20 characters in length.");
            if (errors.Contains(3)) messages.Add("Password must contain at least 1 lowercase character.");
            if (errors.Contains(4)) messages.Add("Password must contain at least 1 uppercase character.");
            if (errors.Contains(5)) messages.Add("Password must contain at least 1 digit.");
            if (errors.Contains(6)) messages.Add("Password must NOT contain white-space.");
            if (errors.Contains(7)) messages.Add("Password must contain at least 1 of these special characters: !@#$%^&*_+.");

            return messages;
        }
    }
}