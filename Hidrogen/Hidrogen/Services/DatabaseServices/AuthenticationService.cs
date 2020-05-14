using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt;
using HelperLibrary;
using HelperLibrary.Common;
using Hidrogen.Controllers;
using Hidrogen.DbContexts;
using Hidrogen.Models;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels.Authentication;
using Hidrogen.ViewModels.Authorization;
using MethaneLibrary.Interfaces;
using MethaneLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Hidrogen.Services.DatabaseServices {

    public class AuthenticationService : IAuthenticationService {

        private readonly ILogger<AuthenticationService> _logger;
        private readonly IRuntimeLogService _runtimeLogger;
        private readonly HidrogenDbContext _dbContext;

        public AuthenticationService(
            ILogger<AuthenticationService> logger,
            IRuntimeLogService runtimeLogger,
            HidrogenDbContext dbContext
        ) {
            _logger = logger;
            _runtimeLogger = runtimeLogger;
            _dbContext = dbContext;
        }

        public async Task<KeyValuePair<bool, bool?>> ActivateHidrogenianAccount(AccountActivationVM activator) {
            _logger.LogInformation("AuthenticationService.ActivateHidrogenianAccount - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationService),
                Action = nameof(ActivateHidrogenianAccount),
                Data = JsonConvert.SerializeObject(activator),
                Briefing = "Query database to activate a user account.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var dbHidrogenian = await _dbContext.Hidrogenian.FirstOrDefaultAsync(
                h => h.Email == activator.Email && !h.EmailConfirmed && h.DeactivatedOn == null
            );

            if (dbHidrogenian == null) return new KeyValuePair<bool, bool?>(false, null);

            if (dbHidrogenian.RecoveryToken == activator.ActivationToken &&
                dbHidrogenian.TokenSetOn.Value.AddHours(24) > DateTime.UtcNow) {
                dbHidrogenian.RecoveryToken = null;
                dbHidrogenian.TokenSetOn = null;
                dbHidrogenian.EmailConfirmed = true;

                _dbContext.Hidrogenian.Update(dbHidrogenian);

                try {
                    await _dbContext.SaveChangesAsync();
                } catch (Exception e) {
                    _logger.LogError("AuthenticationService.ActivateHidrogenianAccount - Error: " + e);
                    await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                        Controller = nameof(AuthenticationService),
                        Action = nameof(ActivateHidrogenianAccount),
                        Briefing = "Exception occurred while saving account data after activation: " + e,
                        Severity = HidroEnums.LOGGING.ERROR.GetValue()
                    });
                    
                    return new KeyValuePair<bool, bool?>(true, null);
                }

                return new KeyValuePair<bool, bool?>(true, true);
            }

            return new KeyValuePair<bool, bool?>(true, false);
        }

        public async Task<KeyValuePair<bool, AuthenticatedUser>> AuthenticateHidrogenian(AuthenticationVM auth) {
            _logger.LogInformation("AuthenticationService.AuthenticateHidrogenian - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationService),
                Action = nameof(AuthenticateHidrogenian),
                Briefing = "Query database to authenticate a user with credentials.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            Hidrogenian hidrogenian;
            string role;
            try {
                hidrogenian = await _dbContext.Hidrogenian.FirstOrDefaultAsync(
                    h => (auth.Email != null ? h.Email == auth.Email
                                             : h.UserName.ToLower() == auth.UserName
                         ) &&
                         h.EmailConfirmed &&
                         h.RecoveryToken == null &&
                         h.TokenSetOn == null
                );

                role = await (from rc in _dbContext.RoleClaimer
                              join r in _dbContext.HidroRole
                                    on rc.RoleId equals r.Id
                              where rc.HidrogenianId == hidrogenian.Id
                              select r.RoleName).FirstOrDefaultAsync();
            } catch (Exception e) {
                _logger.LogError("AuthenticationService.AuthenticateHidrogenian - Error: " + e);
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(AuthenticationService),
                    Action = nameof(AuthenticateHidrogenian),
                    Briefing = "Exception occurred while reading user and role from database: " + e,
                    Severity = HidroEnums.LOGGING.ERROR.GetValue()
                });
                
                return new KeyValuePair<bool, AuthenticatedUser>(false, null);
            }

            if (!BCryptHelper.CheckPassword(auth.Password, hidrogenian.PasswordHash))
                return new KeyValuePair<bool, AuthenticatedUser>(true, null);

            var unixTimeStamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            var authToken = GenerateHashedPasswordAndSalt(hidrogenian.Id + hidrogenian.Email + unixTimeStamp);

            var profile = await _dbContext.HidroProfile.FirstOrDefaultAsync(p => p.HidrogenianId == hidrogenian.Id);
            var expirationTime = ((DateTimeOffset)DateTime.UtcNow.AddSeconds(
                                    auth.TrustedAuth ? HidroConstants.TRUSTED_AUTH_EXPIRATION_TIME : HidroConstants.INTRUSTED_AUTH_EXPIRATION_TIME
                                 )).ToUnixTimeSeconds();

            var avatar = profile.ProcessAvatarInfo();

            var authUser = new AuthenticatedUser {
                UserId = hidrogenian.Id,
                Role = role,
                AuthToken = authToken.Key,
                Email = hidrogenian.Email,
                UserName = hidrogenian.UserName,
                FullName = profile.GivenName + ' ' + profile.FamilyName,
                Avatar = avatar?.Name,
                ExpirationTime = expirationTime
            };

            return new KeyValuePair<bool, AuthenticatedUser>(true, authUser);
        }

        public async Task<KeyValuePair<bool, AuthenticatedUser>> AuthenticateWithCookie(CookieAuthenticationVM cookie) {
            _logger.LogInformation("AuthenticationService.AuthenticateWithCookie - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationService),
                Action = nameof(AuthenticateWithCookie),
                Briefing = "Query database to authenticate a user with cookie.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            Hidrogenian dbHidrogenian;
            string role;
            try {
                dbHidrogenian = await _dbContext.Hidrogenian.FirstOrDefaultAsync(
                    h => h.CookieToken != null && h.CookieToken == cookie.CookieToken &&
                         h.EmailConfirmed && h.RecoveryToken == null &&
                         h.TokenSetOn == null && h.CookieSetOn != null &&
                         ((DateTimeOffset)h.CookieSetOn.Value).ToUnixTimeSeconds() == cookie.TimeStamp
                );

                role = await (from rc in _dbContext.RoleClaimer
                              join r in _dbContext.HidroRole
                                    on rc.RoleId equals r.Id
                              where rc.HidrogenianId == dbHidrogenian.Id
                              select r.RoleName).FirstOrDefaultAsync();
            } catch (Exception e) {
                _logger.LogError("AuthenticationService.AuthenticateWithCookie - Error: " + e);
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(AuthenticationService),
                    Action = nameof(AuthenticateWithCookie),
                    Briefing = "Exception occurred while reading user and role from database: " + e,
                    Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
                });
                
                return new KeyValuePair<bool, AuthenticatedUser>(false, null);
            }

            var unixTimeStamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            var authToken = GenerateHashedPasswordAndSalt(dbHidrogenian.Id + dbHidrogenian.Email + unixTimeStamp);

            var profile = await _dbContext.HidroProfile.FirstOrDefaultAsync(p => p.HidrogenianId == dbHidrogenian.Id);
            var expirationTime = ((DateTimeOffset)DateTime.UtcNow.AddSeconds(
                                    cookie.TrustedAuth == "True" ? HidroConstants.TRUSTED_AUTH_EXPIRATION_TIME : HidroConstants.INTRUSTED_AUTH_EXPIRATION_TIME
                                 )).ToUnixTimeSeconds();

            var avatar = profile.ProcessAvatarInfo();

            var authUser = new AuthenticatedUser {
                UserId = dbHidrogenian.Id,
                Role = role,
                AuthToken = authToken.Key,
                Email = dbHidrogenian.Email,
                FullName = profile.GivenName + ' ' + profile.FamilyName,
                Avatar = avatar.Name,
                ExpirationTime = expirationTime
            };

            return new KeyValuePair<bool, AuthenticatedUser>(true, authUser);
        }

        public async Task<HidroPermissionVM> ComputeAuthorizationFor(int hidrogenianId) {
            _logger.LogInformation("AuthenticationService.ComputeAuthorizationFor - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationService),
                Action = nameof(ComputeAuthorizationFor),
                Briefing = "Query database to get user's roleClaim then set permissions for hidrogenianId = " + hidrogenianId,
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var roleClaim = await _dbContext.RoleClaimer.FirstOrDefaultAsync(rc => rc.HidrogenianId == hidrogenianId);
            HidroPermissionVM permissions = roleClaim;

            if (roleClaim.AllowTemporarily == null) return permissions;

            var temporaryPermissions = JsonConvert.DeserializeObject<TemporaryPermissionVM>(roleClaim.AllowTemporarily);
            if (temporaryPermissions.EffectUntil < DateTime.UtcNow) return permissions;

            if (temporaryPermissions.OverriddenPermissions.Contains(nameof(HidroPermissionVM.AllowCreate)))
                permissions.AllowCreate = !permissions.AllowCreate;

            if (temporaryPermissions.OverriddenPermissions.Contains(nameof(HidroPermissionVM.AllowView)))
                permissions.AllowView = !permissions.AllowView;

            if (temporaryPermissions.OverriddenPermissions.Contains(nameof(HidroPermissionVM.AllowEditOwn)))
                permissions.AllowEditOwn = !permissions.AllowEditOwn;

            if (temporaryPermissions.OverriddenPermissions.Contains(nameof(HidroPermissionVM.AllowEditOthers)))
                permissions.AllowEditOthers = !permissions.AllowEditOthers;

            if (temporaryPermissions.OverriddenPermissions.Contains(nameof(HidroPermissionVM.AllowDeleteOwn)))
                permissions.AllowDeleteOwn = !permissions.AllowDeleteOwn;

            if (temporaryPermissions.OverriddenPermissions.Contains(nameof(HidroPermissionVM.AllowDeleteOthers)))
                permissions.AllowDeleteOthers = !permissions.AllowDeleteOthers;

            if (temporaryPermissions.OverriddenPermissions.Contains(nameof(HidroPermissionVM.AllowReviveOwn)))
                permissions.AllowReviveOwn = !permissions.AllowReviveOwn;

            if (temporaryPermissions.OverriddenPermissions.Contains(nameof(HidroPermissionVM.AllowReviveOthers)))
                permissions.AllowReviveOthers = !permissions.AllowReviveOthers;

            return permissions;
        }

        public async Task<CookieAuthenticationVM> GenerateCookieAuthData(AuthenticatedUser auth) {
            _logger.LogInformation("AuthenticationService.GenerateCookieAuthData - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationService),
                Action = nameof(GenerateCookieAuthData),
                Briefing = "Calculate cookie auth data then query database to save.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var timestamp = DateTime.UtcNow;
            var unixTimestamp = ((DateTimeOffset)timestamp).ToUnixTimeSeconds();

            var cookieAuthToken = auth.AuthToken + auth.Role + unixTimestamp;
            var hashResult = GenerateHashedPasswordAndSalt(cookieAuthToken);

            var dbHidrogenian = await _dbContext.Hidrogenian.FindAsync(auth.UserId);
            dbHidrogenian.CookieToken = hashResult.Key;
            dbHidrogenian.CookieSetOn = timestamp;

            _dbContext.Hidrogenian.Update(dbHidrogenian);
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("AuthenticationService.GenerateCookieAuthData - Error: " + e);
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(AuthenticationService),
                    Action = nameof(GenerateCookieAuthData),
                    Briefing = "Exception occurred while saving cookie auth data: " + e,
                    Severity = HidroEnums.LOGGING.ERROR.GetValue()
                });
                
                return null;
            }

            return new CookieAuthenticationVM {
                CookieToken = dbHidrogenian.CookieToken,
                TimeStamp = unixTimestamp
            };
        }

        public KeyValuePair<string, string> GenerateHashedPasswordAndSalt(string plainText) {
            _logger.LogInformation("AuthenticationService.GenerateHashedPasswordAndSalt - Service starts.");
            _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationService),
                Action = nameof(GenerateHashedPasswordAndSalt),
                Briefing = "Calculate the hashed password from plain text.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var salt = BCryptHelper.GenerateSalt();
            var hashedPassword = BCryptHelper.HashPassword(plainText, salt);

            return new KeyValuePair<string, string>(hashedPassword, salt);
        }

        public string GenerateRandomToken() {
            _logger.LogInformation("AuthenticationService.GenerateRandomToken - Service starts.");
            _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationService),
                Action = nameof(GenerateRandomToken),
                Briefing = "Generate a random salt string.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });
            return BCryptHelper.GenerateSalt(18, SaltRevision.Revision2A);
        }

        public async Task<bool?> IsEmailAddressAvailable(string email) {
            _logger.LogInformation("AuthenticationService.IsEmailAddressAvailable - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationService),
                Action = nameof(IsEmailAddressAvailable),
                Briefing = "Query database to check if email address is not prior registered: " + email,
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            bool available;
            try {
                available = !(await _dbContext.Hidrogenian.AnyAsync(h => h.Email == email));
            } catch (Exception e) {
                _logger.LogError("AuthenticationService.IsEmailAddressAvailable - Error: " + e);
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(AuthenticationService),
                    Action = nameof(IsEmailAddressAvailable),
                    Briefing = "Exception occurred while reading database: " + e,
                    Severity = HidroEnums.LOGGING.ERROR.GetValue()
                });
                
                return null;
            }

            return available;
        }

        public async Task<bool?> IsUserNameAvailable(string username) {
            _logger.LogInformation("AuthenticationService.IsUserNameAvailable - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationService),
                Action = nameof(IsUserNameAvailable),
                Briefing = "Query database to check if username is not prior registered: " + username,
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            bool available;
            try {
                available = !(await _dbContext.Hidrogenian.AnyAsync(h => h.UserName.ToLower() == username.ToLower()));
            } catch (Exception e) {
                _logger.LogError("AuthenticationService.IsUserNameAvailable - Error: " + e);
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(AuthenticationService),
                    Action = nameof(IsUserNameAvailable),
                    Briefing = "Exception occurred while reading database: " + e,
                    Severity = HidroEnums.LOGGING.ERROR.GetValue()
                });
                
                return null;
            }

            return available;
        }

        public async Task<KeyValuePair<bool, bool?>?> ReplaceAccountPassword(RegistrationVM recovery) {
            _logger.LogInformation("AuthenticationService.ReplaceAccountPassword - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationService),
                Action = nameof(ReplaceAccountPassword),
                Briefing = "Query database to change account password.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var hidrogenian = await _dbContext.Hidrogenian.FirstOrDefaultAsync(
                h => h.Email == recovery.Email && !h.EmailConfirmed && h.RecoveryToken != null && h.TokenSetOn != null
            );

            if (hidrogenian == null) return null;

            if (hidrogenian.RecoveryToken == recovery.RecoveryToken &&
                hidrogenian.TokenSetOn.Value.AddMinutes(30) > DateTime.UtcNow) {
                if (!BCryptHelper.CheckPassword(recovery.TempPassword, hidrogenian.PasswordHash))
                    return new KeyValuePair<bool, bool?>(true, null);

                var (key, value) = GenerateHashedPasswordAndSalt(recovery.Password);
                hidrogenian.PasswordHash = key;
                hidrogenian.PasswordSalt = value;

                hidrogenian.EmailConfirmed = true;
                hidrogenian.RecoveryToken = null;
                hidrogenian.TokenSetOn = null;

                _dbContext.Hidrogenian.Update(hidrogenian);
                try {
                    await _dbContext.SaveChangesAsync();
                } catch (Exception e) {
                    _logger.LogError("AuthenticationService.ReplaceAccountPassword - Error: " + e);
                    await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                        Controller = nameof(AuthenticationService),
                        Action = nameof(ReplaceAccountPassword),
                        Briefing = "Exception occurred while saving data: " + e,
                        Severity = HidroEnums.LOGGING.ERROR.GetValue()
                    });
                    
                    return new KeyValuePair<bool, bool?>(true, false);
                }

                return new KeyValuePair<bool, bool?>(true, true);
            }

            return new KeyValuePair<bool, bool?>(false, null);
        }

        public async Task<KeyValuePair<string, string>> SetTempPasswordAndRecoveryToken(RecoveryVM recoveree) {
            _logger.LogInformation("AuthenticationService.SetTempPasswordAndRecoveryToken - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationService),
                Action = nameof(SetTempPasswordAndRecoveryToken),
                Briefing = "Query database to get an account then set temp password for it.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var dbHidrogenian = !recoveree.Reattempt ? await _dbContext.Hidrogenian.FirstOrDefaultAsync(
                                                            h => h.Email == recoveree.Email && h.EmailConfirmed && h.DeactivatedOn == null)
                                                     : await _dbContext.Hidrogenian.FirstOrDefaultAsync(
                                                            h => h.Email == recoveree.Email && !h.EmailConfirmed && h.DeactivatedOn == null &&
                                                            h.RecoveryToken != null && h.TokenSetOn != null);

            if (dbHidrogenian == null) return new KeyValuePair<string, string>(null, null);

            var tempPassword = HelperProvider.GenerateRandomString(15);
            var hashedResult = GenerateHashedPasswordAndSalt(tempPassword);

            dbHidrogenian.PasswordHash = hashedResult.Key;
            dbHidrogenian.PasswordSalt = hashedResult.Value;

            var recoveryToken = GenerateRandomToken();
            dbHidrogenian.RecoveryToken = recoveryToken;
            dbHidrogenian.TokenSetOn = DateTime.UtcNow;

            dbHidrogenian.EmailConfirmed = false;

            _dbContext.Hidrogenian.Update(dbHidrogenian);
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("AuthenticationService.SetTempPasswordAndRecoveryToken - Error: " + e);
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(AuthenticationService),
                    Action = nameof(SetTempPasswordAndRecoveryToken),
                    Briefing = "Exception occurred while saving data: " + e,
                    Severity = HidroEnums.LOGGING.ERROR.GetValue()
                });
                
                return new KeyValuePair<string, string>(string.Empty, null);
            }

            return new KeyValuePair<string, string>(tempPassword, recoveryToken);
        }

        public async Task<bool?> VerifyAccountPasswordFor(int hidrogenianId, string password) {
            _logger.LogInformation("AuthenticationService.VerifyAccountPasswordFor - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(AuthenticationService),
                Action = nameof(SetTempPasswordAndRecoveryToken),
                Briefing = "Query database to get an account then check if an arbitrary password matches its password.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var account = await _dbContext.Hidrogenian.FindAsync(hidrogenianId);
            if (account == null) return null;

            return BCryptHelper.CheckPassword(password, account.PasswordHash);
        }
    }
}
