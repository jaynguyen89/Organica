using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hidrogen.Services.Interfaces {

    public interface IAuthenticationService {

        KeyValuePair<string, string> GenerateHashedPasswordAndSalt(string plainText);

        string GenerateRandomToken();

        Task<bool?> IsUserNameAvailable(string username);

        Task<bool?> IsEmailAddressAvailable(string email);
    }
}