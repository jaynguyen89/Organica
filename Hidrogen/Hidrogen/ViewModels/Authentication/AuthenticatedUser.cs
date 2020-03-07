namespace Hidrogen.ViewModels {

    public class AuthenticatedUser {

        public string AuthToken { get; set; }

        public int ExpirationTime { get; set; } // in milliseconds

        public int UserId { get; set; }

        public string Role { get; set; }
    }
}
