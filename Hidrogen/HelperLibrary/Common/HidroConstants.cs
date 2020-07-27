using System.Collections.Generic;

namespace HelperLibrary.Common {

    public static class HidroConstants {

        public const string PROJECT_NAME = "HidrogenStore";

        public const int EMAIL_VALIDITY_DURATION = 24; //hours

        public const int TRUSTED_AUTH_EXPIRATION_TIME = 6 * 60 * 60; // 6 hours
        public const int INTRUSTED_AUTH_EXPIRATION_TIME = 1 * 60 * 60; // 1 hour

        public const long CLIENT_COOKIE_EXPIRATION_TIME = 7 * 24 * 60 * 60; // 7 days

        public const string DOUBLE_SPACE = "  ";
        public const string WHITE_SPACE = " ";
        public const int EMPTY = 0;
        public const string NA = "N/A";

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

        public const int CACHE_SLIDING_EXPIRATION_TIME = 3; //days
        public const int CACHE_ABSOLUTE_EXPIRATION_TIME = 30; //days

        private static readonly List<string> ROLES = new List<string> {
            "customer", "supporter", "moderator", "administrator"
        };

        public static int GetRoleHierarchy(string role) {
            return ROLES.IndexOf(role);
        }

        public static Dictionary<string, string> API_TOKEN_TARGETS = new Dictionary<string, string> {
            { "save_avatar", "Avatar/saveAvatar" },
            { "replace_avatar", "Avatar/replaceAvatar" },
            { "remove_avatar", "Avatar/removeAvatar" }
        };

        public static Dictionary<string, string> CONTENT_TYPES = new Dictionary<string, string> {
            { "json", "application/json" },
            { "form", "multipart/form-data" },
            { "xml", "application/xml" },
            { "mixed", "multipart/mixed" },
            { "alt", "multipart/alternative" },
            { "base64", "application/base64" }
        };
        
        public static Dictionary<string, string> PERMISSION_VALS = new Dictionary<string, string> {
            { "create", "AllowCreate" },
            { "view", "AllowView" },
            { "edit_own", "AllowEditOwn" },
            { "edit_others", "AllowEditOthers" },
            { "delete_own", "AllowDeleteOwn" },
            { "delete_others", "AllowDeleteOthers" },
            { "revive_own", "AllowReviveOwn" },
            { "revive_others", "AllowReviveOthers" }
        };
        
        public static Dictionary<string, string> CACHE_ENTRY_KEYS = new Dictionary<string, string> {
            { "Profile_AddressList", "Profile_AddressList" },
            { "Account_IdentityDetail", "Account_IdentityDetail" },
            { "Account_2FAData", "Account_2FAData" },
            { "Account_TimeStamps", "Account_TimeStamps" },
            { "Country_CompactList", "Country_CompactList" },
            { "Account_PaymentDetails", "Account_PaymentDetails" },
            { "Profile_AvatarInfo", "Profile_AvatarInfo" },
            { "Profile_PrivateProfile", "Profile_PrivateProfile" },
            { "HidrogenianService_UnactivatedUser", "HidrogenianService_UnactivatedUser" }
        };
    }
}