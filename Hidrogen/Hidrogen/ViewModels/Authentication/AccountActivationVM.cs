namespace Hidrogen.ViewModels {

    public class AccountActivationVM {

        public string Email { get; set; }

        public string ActivationToken { get; set; }

        public string CaptchaToken { get; set; }
    }
}