using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelperLibrary;
using Hidrogen.Controllers;
using Hidrogen.DbContexts;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels.Account;
using MethaneLibrary.Interfaces;
using MethaneLibrary.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Hidrogen.Services.DatabaseServices {

    public class AccountService : IAccountService {

        private readonly ILogger<AccountService> _logger;
        private readonly IRuntimeLogService _runtimeLogger;
        private readonly HidrogenDbContext _dbContext;

        public AccountService(
            ILogger<AccountService> logger,
            IRuntimeLogService runtimeLogger,
            HidrogenDbContext dbContext
        ) {
            _logger = logger;
            _runtimeLogger = runtimeLogger;
            _dbContext = dbContext;
        }

        public async Task<AccountIdentityVM> GetAccountIdentity(int hidrogenianId) {
            _logger.LogInformation("AccountService.GetAccountIdentity - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AccountService),
                Action = nameof(GetAccountIdentity),
                Briefing = "Query database to get account identity for user having hidrogenianId = " + hidrogenianId,
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var account = await _dbContext.Hidrogenian.FindAsync(hidrogenianId);
            return account;
        }

        public async Task<TimeStampVM> GetAccountTimeStamps(int hidrogenianId) {
            _logger.LogInformation("AccountService.GetAccountTimeStamps - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AccountService),
                Action = nameof(GetAccountTimeStamps),
                Briefing = "Query database to get account timestamps for user having hidrogenianId = " + hidrogenianId,
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var account = await _dbContext.Hidrogenian.FindAsync(hidrogenianId);
            return account;
        }

        public async Task<string> RetrieveTwoFaSecretKeyFor(int hidrogenianId) {
            _logger.LogInformation("AccountService.RetrieveTwoFaSecretKeyFor - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AccountService),
                Action = nameof(RetrieveTwoFaSecretKeyFor),
                Briefing = "Query database to get account 2FA secret key for user having hidrogenianId = " + hidrogenianId,
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });
            
            var account = await _dbContext.Hidrogenian.FindAsync(hidrogenianId);
            if (account == null) return null;

            return account.TwoFactorEnabled ? account.TwoFaSecretKey : string.Empty;
        }

        public async Task<bool> ReverseIdentityChanges(AccountIdentityVM oldIdentity) {
            _logger.LogInformation("AccountService.ReverseIdentityChanges - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AccountService),
                Action = nameof(ReverseIdentityChanges),
                Briefing = "Query database to put back old identity data after service failed.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

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
                
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(AccountService),
                    Action = nameof(ReverseIdentityChanges),
                    Briefing = "Exception occurred while saving old identity.",
                    Severity = HidroEnums.LOGGING.ERROR.GetValue()
                });
                
                return false;
            }

            return true;
        }

        public async Task<KeyValuePair<bool, KeyValuePair<AccountIdentityVM, AccountIdentityVM>?>> UpdateIdentityForHidrogenian(AccountIdentityVM identity) {
            _logger.LogInformation("AccountService.UpdateIdentityForHidrogenian - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AccountService),
                Action = nameof(UpdateIdentityForHidrogenian),
                Data = JsonConvert.SerializeObject(identity),
                Briefing = "Query database to update identity for user.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

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
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(AccountService),
                    Action = nameof(UpdateIdentityForHidrogenian),
                    Briefing = "Exception occurred while saving new identity.",
                    Severity = HidroEnums.LOGGING.ERROR.GetValue()
                });
                
                return new KeyValuePair<bool, KeyValuePair<AccountIdentityVM, AccountIdentityVM>?>(true, null);
            }

            return new KeyValuePair<bool, KeyValuePair<AccountIdentityVM, AccountIdentityVM>?>(true, new KeyValuePair<AccountIdentityVM, AccountIdentityVM>(oldIdentity, identity));
        }

        public async Task<bool?> UpdatePasswordForAccount(AccountSecurityVM security) {
            _logger.LogInformation("AccountService.UpdatePasswordForAccount - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AccountService),
                Action = nameof(UpdatePasswordForAccount),
                Briefing = "Query database to save new security data.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var dbAccount = await _dbContext.Hidrogenian.FindAsync(security.Id);
            if (dbAccount == null) return null;

            dbAccount.PasswordHash = security.NewPassword;
            dbAccount.PasswordSalt = security.PasswordConfirm;

            _dbContext.Hidrogenian.Update(dbAccount);
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("AccountService.UpdatePasswordForAccount - Error: " + e);
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(AccountService),
                    Action = nameof(UpdatePasswordForAccount),
                    Briefing = "Exception occurred while saving new security data.",
                    Severity = HidroEnums.LOGGING.ERROR.GetValue()
                });
                
                return false;
            }

            return true;
        }
    }
}
