namespace Hidrogen.ViewModels {

    public class AuthenticatedUser {

        public string AuthToken { get; set; }

        public int ExpirationTime { get; set; } // in milliseconds

        public int UserId { get; set; }

        public string Role { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Avatar { get; set; }
    }
}
