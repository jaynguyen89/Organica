namespace Hidrogen.ViewModels {

    public class RecoveryVM {

        public string Email { get; set; }

        public string CaptchaToken { get; set; }

        public bool Reattempt { get; set; }
    }
}