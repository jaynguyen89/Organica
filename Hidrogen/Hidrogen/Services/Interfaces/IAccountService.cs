using Hidrogen.ViewModels.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hidrogen.Services.Interfaces {

    public interface IAccountService {

        /// <summary>
        /// Returns Key1-Value1(Key2-Value2) pair. Key1 == false if no account found with the given data. When Key1 == true, Value1 == null if database update failed.
        /// Otherwise, Key2 holds old AccountIdentity data, Value2 holds new AccountIdentity data.
        /// </summary>
        Task<KeyValuePair<bool, KeyValuePair<AccountIdentityVM, AccountIdentityVM>?>> UpdateIdentityForHidrogenian(AccountIdentityVM identity);

        /// <summary>
        /// Returns null if no hidrogenian found, returns false if database update failed, otherwise returns true.
        /// </summary>
        Task<bool?> UpdatePasswordForAccount(AccountSecurityVM security);

        /// <summary>
        /// Returns null if no hidrogenian found with given ID, otherwise, returns an instance of AccountIdentityVM.
        /// </summary>
        Task<AccountIdentityVM> GetAccountIdentity(int hidrogenianId);

        /// <summary>
        /// Returns null if no hidrogenian found with given ID, otherwise, returns an instance of TimeStampVM.
        /// </summary>
        Task<TimeStampVM> GetAccountTimeStamps(int hidrogenianId);

        /// <summary>
        /// Returns null if no hidrogenian found, returns false if database update failed, otherwise returns true.
        /// </summary>
        Task<bool> ReverseIdentityChanges(AccountIdentityVM oldIdentity);

        /// <summary>
        /// Returns null if no hidrogenian found, otherwise returns the secret key string.
        /// </summary>
        Task<string> RetrieveTwoFaSecretKeyFor(int hidrogenianId);
    }
}