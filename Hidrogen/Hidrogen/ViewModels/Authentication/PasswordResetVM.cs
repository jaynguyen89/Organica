namespace Hidrogen.ViewModels.Authentication {

    public class PasswordResetVM {

        public string Email { get; set; }

        public string Password { get; set; } = null;

        public string PasswordConfirm { get; set; }
    }
}
