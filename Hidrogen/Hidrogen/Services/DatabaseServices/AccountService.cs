using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hidrogen.DbContexts;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels.Account;
using Microsoft.Extensions.Logging;

namespace Hidrogen.Services.DatabaseServices {

    public class AccountService : IAccountService {

        private readonly ILogger<AccountService> _logger;
        private readonly HidrogenDbContext _dbContext;

        public AccountService(
            ILogger<AccountService> logger,
            HidrogenDbContext dbContext
        ) {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<AccountIdentityVM> GetAccountIdentity(int hidrogenianId) {
            _logger.LogInformation("AccountService.GetAccountIdentity - Service starts.");

            var account = await _dbContext.Hidrogenian.FindAsync(hidrogenianId);
            return account;
        }

        public async Task<TimeStampVM> GetAccountTimeStamps(int hidrogenianId) {
            _logger.LogInformation("AccountService.GetAccountTimeStamps - Service starts.");

            var account = await _dbContext.Hidrogenian.FindAsync(hidrogenianId);
            return account;
        }

        public async Task<string> RetrieveTwoFaSecretKeyFor(int hidrogenianId) {
            _logger.LogInformation("AccountService.RetrieveTwoFaSecretKeyFor - Service starts.");
            
            var account = await _dbContext.Hidrogenian.FindAsync(hidrogenianId);
            if (account == null) return null;

            return account.TwoFactorEnabled ? account.TwoFaSecretKey : string.Empty;
        }

        public async Task<bool> ReverseIdentityChanges(AccountIdentityVM oldIdentity) {
            _logger.LogInformation("AccountService.ReverseIdentityChanges - Service starts.");

            var dbAccount = await _dbContext.Hidrogenian.FindAsync(oldIdentity.Id);

            dbAccount.Email = oldIdentity.Email;
            dbAccount.EmailConfirmed = oldIdentity.EmailConfirmed;
            dbAccount.UserName = oldIdentity.UserName;
            dbAccount.PhoneNumber = oldIdentity.PhoneNumber;
            dbAccount.PhoneNumberConfirmed = oldIdentity.PhoneConfirmed;

            dbAccount.RecoveryToken = null;
            dbAccount.TokenSetOn = null;

            _dbContext.Hidrogenian.Update(dbAccount);
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogInformation("AccountService.ReverseIdentityChanges - Error: " + e);
                return false;
            }

            return true;
        }

        public async Task<KeyValuePair<bool, KeyValuePair<AccountIdentityVM, AccountIdentityVM>?>> UpdateIdentityForHidrogenian(AccountIdentityVM identity) {
            _logger.LogInformation("AccountService.UpdateIdentityForHidrogenian - Service starts.");

            var account = await _dbContext.Hidrogenian.FindAsync();
            if (account == null) return new KeyValuePair<bool, KeyValuePair<AccountIdentityVM, AccountIdentityVM>?>(false, null);

            AccountIdentityVM oldIdentity = account;

            if (account.Email != identity.Email) {
                account.Email = identity.Email;
                account.EmailConfirmed = false;
                identity.EmailConfirmed = false;
            }

            if (account.PhoneNumber != identity.PhoneNumber) {
                account.PhoneNumber = identity.PhoneNumber;
                account.PhoneNumberConfirmed = false;
                identity.PhoneConfirmed = false;
            }

            account.UserName = identity.UserName;
            _dbContext.Hidrogenian.Update(account);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("AccountService.UpdateIdentityForHidrogenian - Error: " + e);
                return new KeyValuePair<bool, KeyValuePair<AccountIdentityVM, AccountIdentityVM>?>(true, null);
            }

            return new KeyValuePair<bool, KeyValuePair<AccountIdentityVM, AccountIdentityVM>?>(true, new KeyValuePair<AccountIdentityVM, AccountIdentityVM>(oldIdentity, identity));
        }

        public async Task<bool?> UpdatePasswordForAccount(AccountSecurityVM security) {
            _logger.LogInformation("AccountService.UpdatePasswordForAccount - Service starts.");

            var dbAccount = await _dbContext.Hidrogenian.FindAsync(security.Id);
            if (dbAccount == null) return null;

            dbAccount.PasswordHash = security.NewPassword;
            dbAccount.PasswordSalt = security.PasswordConfirm;

            _dbContext.Hidrogenian.Update(dbAccount);
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("AccountService.UpdatePasswordForAccount - Error: " + e);
                return false;
            }

            return true;
        }
    }
}
