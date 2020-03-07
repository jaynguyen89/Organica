namespace HelperLibrary.Common {

    public class HidroConstants {

        public const int TRUSTED_AUTH_EXPIRATION_TIME = 6 * 60 * 60 * 1000; // 6 hours
        public const int INTRUSTED_AUTH_EXPIRATION_TIME = 1 * 60 * 60 * 1000; // 1 hour

        public const long CLIENT_COOKIE_EXPIRATION_TIME = 7 * 24 * 60 * 60 * 1000; // 7 days

        public const string WHITE_SPACE = " ";

        public const string GOOGLE_CAPTCHA_ENDPOINT = @"https://www.google.com/recaptcha/api/siteverify";
        public const string GOOGLE_CAPTCHA_SECRET_KEY = "6LeXhN4UAAAAADblMiFrLL6v0WM3pNIkHyfaoCg5";

        public const string MAIL_SERVER_HOST = "smtp.gmail.com";
        public const int MAIL_SERVER_PORT = 587;
        public const bool MAIL_SERVER_TLS = true;
        public const bool USE_DEFAULT_CREDENTIALS = false;
        public const string MAIL_SERVER_USERNAME = "nguyen.le.kim.phuc@gmail.com";
        public const string MAIL_SERVER_PASSWORD = "Chay571990";
        public const string MAIL_SENDER_ADDRESS = "nguyen.le.kim.phuc@gmail.com";
        public const string MAIL_SENDER_NAME = "Hidrogen";
    }
}
