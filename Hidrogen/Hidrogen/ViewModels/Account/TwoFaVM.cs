namespace Hidrogen.ViewModels.Account {

    public class TwoFaVM {

        public int Id { get; set; }

        public string QrImageUrl { get; set; }

        public string ManualQrCode { get; set; }

        public string CaptchaToken { get; set; }
    }
}
