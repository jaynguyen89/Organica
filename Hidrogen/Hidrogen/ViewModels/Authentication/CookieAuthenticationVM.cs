namespace Hidrogen.ViewModels.Authentication {

    public class CookieAuthenticationVM {

        public string CookieToken { get; set; }

        public long TimeStamp { get; set; }

        public string TrustedAuth { get; set; }
    }
}
