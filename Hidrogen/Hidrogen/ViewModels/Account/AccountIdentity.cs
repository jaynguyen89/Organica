namespace Hidrogen.ViewModels.Account {

    public class AccountIdentity {

        public int Id { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string Username { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneConfirmed { get; set; }
    }
}