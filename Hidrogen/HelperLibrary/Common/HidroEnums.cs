using System.Runtime.Serialization;
using static HelperLibrary.Common.HidroAttributes;

namespace HelperLibrary {

    public static class HidroEnums {

        public enum ROLES {
            [StringValue("Guest")]
            GUEST,
            [StringValue("Customer")]
            CUSTOMER,
            [StringValue("Supporter")]
            SUPPORTER,
            [StringValue("Moderator")]
            MOD,
            [StringValue("Administrator")]
            ADMIN
        }

        public enum PERMISSIONS {
            [StringValue("AllowCreate")]
            CREATE,
            [StringValue("AllowView")]
            VIEW,
            [StringValue("AllowEditOwn")]
            EDIT_OWN,
            [StringValue("AllowEditOthers")]
            EDIT_OTHERS,
            [StringValue("AllowDeleteOwn")]
            DELETE_OWN,
            [StringValue("AllowDeleteOthers")]
            DELETE_OTHERS,
            [StringValue("AllowReviveOwn")]
            REVIVE_OWN,
            [StringValue("AllowReviveOthers")]
            REVIVE_OTHERS
        }

        public enum LOGGING {
            [StringValue("Information")]
            INFORMATION,
            [StringValue("Warning")]
            WARNING,
            [StringValue("Error")]
            ERROR,
            [StringValue("Fatal")]
            FATAL
        }

        public enum RESULTS {
            FAILED, SUCCESS, INTERRUPTED
        }

        public enum FILTER_RESULT {
            INVALID_AUTHENTICATION,
            AUTHENTICATION_EXPIRED,
            ACCESS_CONTROL_DENIED,
            INSUFFICIENT_PERMISSION
        }

        public enum PAYMENT_METHODS {
            [StringValue("card")]
            CARD,
            [StringValue("paypal")]
            PAYPAL
        }

        public enum HTTP_STATUS_CODES {
            CONTINUE = 100,
            SWITCHING_PROTOCOL = 101,
            PROCESSING = 102,
            EARLY_HINTS = 103,
            OK = 200,
            CREATED = 201,
            ACCEPTED = 202,
            NONAUTHORITATIVE_INFORMATION = 203,
            NO_CONTENT = 204,
            RESET_CONTENT = 205,
            PARTIAL_CONTENT = 206,
            MULTI_STATUS = 207,
            ALREADY_REPORTED = 208,
            IM_USED = 226,
            MULTIPLE_CHOICES = 300,
            MOVED_PERMANENTLY = 301,
            FOUND = 302, //previously Moved Temporarily
            SEE_OTHER = 303,
            NOT_MODIFIED = 304,
            USE_PROXY = 305,
            SWITCH_PROXY = 306,
            TEMPORARY_REDIRECT = 307,
            PERMANENT_REDIRECT = 308,
            BAD_REQUEST = 400,
            UNAUTHORIZED = 401,
            PAYMENT_REQUIRED = 402,
            FORBIDDEN = 403,
            NOT_FOUND = 404,
            METHOD_NOT_ALLOWED = 405,
            NOT_ACCEPTABLE = 406,
            PROXY_AUTHENTICATION_REQUIRED = 407,
            REQUEST_TIMEOUT = 408,
            CONFLICT = 409,
            GONE = 410,
            LENGTH_REQUIRED = 411,
            PRECONDITION_FAILED = 412,
            PAYLOAD_TOO_LARGE = 413,
            URI_TOO_LONG = 414,
            UNSUPPORTED_MEDIA_TYPE = 415,
            RANGE_NOT_SATISFIABLE = 416,
            EXPECTATION_FAILED = 417,
            IM_A_TEAPOT = 418,
            MISREDIRECTED_REQUEST = 421,
            UNPROCESSABLE_ENTITY = 422,
            LOCKED = 423,
            FAILED_DEPENDENCY = 424,
            TOO_EARLY = 425,
            UPGRADE_REQUIRED = 426,
            PRECONDITION_REQUIRED = 428,
            TOO_MANY_REQUESTS = 429,
            REQUEST_HEADER_FIELDS_TOO_LARGE = 431,
            UNAVAILABLE_FOR_LEGAL_REASONS = 451,
            INTERNAL_SERVER_ERROR = 500,
            NOT_IMPLEMENTED = 501,
            BAD_GATEWAY = 502,
            SERVICE_UNAVAILABLE = 503,
            GATEWAY_TIMEOUT = 504,
            HTTP_VERSION_NOT_SUPPORTED = 505,
            VARIANT_ALSO_NEGOTIATES = 506,
            INSUFFICIENT_STORAGE = 507,
            LOOP_DETECTED = 508,
            NOT_EXTENDED = 510,
            NETWORK_AUTHENTICATION_REQUIRED = 511,
            INVALID_SSL_CERTIFICATE = 526
        }

        public enum DATE_FORMATS {
            [StringValue("yyyy/MM/dd hh:mm:ss")]
            FULL_YEAR_FIRST,

            [StringValue("dd/MM/yyyy hh:mm:ss")]
            FULL_YEAR_LAST,

            [StringValue("MM/dd/yyyy hh:mm:ss")]
            FULL_MONTH_FIRST,

            [StringValue("yyyy/MM/dd hh:mm")]
            NONSEC_YEAR_FIRST,

            [StringValue("dd/MM/yyyy hh:mm")]
            NONSEC_YEAR_LAST,

            [StringValue("MM/dd/yyyy hh:mm")]
            NONSEC_MONTH_FIRST,

            [StringValue("yyyy/MM/dd")]
            DATE_YEAR_FIRST,

            [StringValue("dd/MM/yyyy")]
            DATE_YEAR_LAST,

            [StringValue("MM/dd/yyyy")]
            DATE_MONTH_FIRST,

            [StringValue("dd MMM yyyy")]
            FULL_DATE_FRIENDLY,

            [StringValue("MMM yyyy")]
            NONDATE_FRIENDLY,

            [StringValue("dd MMM yyyy hh:mm")]
            NONSEC_DATETIME_FRIENDLY,

            [StringValue("yyyy")]
            YEAR_ONLY,

            [StringValue("MM/yyyy")]
            MONTH_YEAR,

            [StringValue("hh:mm:ss")]
            TIME_FULL,

            [StringValue("hh:mm")]
            TIME_NONSEC
        }
    }
}
