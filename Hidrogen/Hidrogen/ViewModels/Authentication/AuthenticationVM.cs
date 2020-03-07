using System.Collections.Generic;

namespace Hidrogen.ViewModels {

    public class AuthenticationVM {

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool TrustedAuth { get; set; }

        public string CaptchaToken { get; set; }

        public List<int> VerifyAuthenticationData() {
            var errors = new List<int>();

            if (UserName != null) UserName = UserName.Trim().ToLower();
            if (Email != null) Email = Email.Trim().ToLower();

            if ((string.IsNullOrEmpty(UserName) && string.IsNullOrEmpty(Email)) ||
                (string.IsNullOrWhiteSpace(UserName) && string.IsNullOrWhiteSpace(Email)))
                errors.Add(0);

            if (string.IsNullOrEmpty(Password) || string.IsNullOrWhiteSpace(Password))
                errors.Add(1);

            return errors; 
        }

        public List<string> GenerateErrorMessages(List<int> validation) {
            var messages = new List<string>();

            if (validation.Contains(0)) messages.Add("Please enter your username or email to login.");
            if (validation.Contains(1)) messages.Add("Please enter your password to login.");

            return messages;
        }
    }
}
