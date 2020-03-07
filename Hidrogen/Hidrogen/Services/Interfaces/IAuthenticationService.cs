using Hidrogen.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hidrogen.Services.Interfaces {

    public interface IAuthenticationService {

        /// <summary>
        /// Returns Key-Value pair with Key being the Hashed Password and Value being the Salt string.
        /// </summary>
        KeyValuePair<string, string> GenerateHashedPasswordAndSalt(string plainText);

        /// <summary>
        /// Returns a random-length salt string.
        /// </summary>
        string GenerateRandomToken();

        /// <summary>
        /// Checks if a username has prior been taken by any account.
        /// </summary>
        Task<bool?> IsUserNameAvailable(string username);

        /// <summary>
        /// Checks if an email address has prior been used to register for any account.
        /// </summary>
        Task<bool?> IsEmailAddressAvailable(string email);

        /// <summary>
        /// Returns Key-Value? pair. Key indicates if a database instance is found by email. When Key == true,
        /// Value == null indicates database transaction failed. Value == false indicates invalid activation token.
        /// </summary>
        Task<KeyValuePair<bool, bool?>> ActivateHidrogenianAccount(AccountActivationVM activator);

        /// <summary>
        /// Returns Key-Value pair. Key == null indicates no account found with the email.
        /// Key == EMPTY indicates database transaction failed. Otherwise, Key holds TempPassword, Value holds RecoveryToken.
        /// </summary>
        Task<KeyValuePair<string, string>> SetTempPasswordAndRecoveryToken(RecoveryVM recoveree);

        /// <summary>
        /// Returns null if no account found by the email. Otherwise, Key == false indicates invalid recovery token.
        /// When Key == true, Value == null indicates invalid TempPassword, Value == false indicates database transaction failed.
        /// </summary>
        Task<KeyValuePair<bool, bool?>?> ReplaceAccountPassword(RegistrationVM recovery);

        /// <summary>
        /// Returns Key == false if no account found with auth data. When Key == true, Value == null if authentication failed.
        /// </summary>
        Task<KeyValuePair<bool, AuthenticatedUser>> AuthenticateHidrogenian(AuthenticationVM auth);
    }
}