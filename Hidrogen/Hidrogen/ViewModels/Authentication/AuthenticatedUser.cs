namespace Hidrogen.ViewModels.Authentication {

    public class AuthenticatedUser {

        public string AuthToken { get; set; }

        public long ExpirationTime { get; set; } // in seconds

        public int UserId { get; set; }

        public string Role { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Avatar { get; set; }
    }
}
