using Hidrogen.Models;
using Hidrogen.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCrypt;
using Microsoft.EntityFrameworkCore;
using System;
using Hidrogen.ViewModels.Authentication;
using HelperLibrary;
using HelperLibrary.Common;
using System.Linq;
using Hidrogen.ViewModels.Authorization;
using Newtonsoft.Json;
using Hidrogen.DbContexts;

namespace Hidrogen.Services.DatabaseServices {

    public class AuthenticationService : IAuthenticationService {

        private readonly ILogger<AuthenticationService> _logger;
        private HidrogenDbContext _dbContext;

        public AuthenticationService(
            ILogger<AuthenticationService> logger,
            HidrogenDbContext dbContext
        ) {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<KeyValuePair<bool, bool?>> ActivateHidrogenianAccount(AccountActivationVM activator) {
            _logger.LogInformation("AuthenticationService.ActivateHidrogenianAccount - Service starts.");

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
                    _logger.LogError("AuthenticationService.ActivateHidrogenianAccount - Error: " + e.ToString());
                    return new KeyValuePair<bool, bool?>(true, null);
                }

                return new KeyValuePair<bool, bool?>(true, true);
            }

            return new KeyValuePair<bool, bool?>(true, false);
        }

        public async Task<KeyValuePair<bool, AuthenticatedUser>> AuthenticateHidrogenian(AuthenticationVM auth) {
            _logger.LogInformation("AuthenticationService.AuthenticateHidrogenian - Service starts.");

            Hidrogenian hidrogenian = null;
            var role = string.Empty;
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
                _logger.LogError("AuthenticationService.AuthenticateHidrogenian - Error: " + e.ToString());
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

            var avatarInfo = profile.ProcessAvatarInfo();

            var authUser = new AuthenticatedUser {
                UserId = hidrogenian.Id,
                Role = role,
                AuthToken = authToken.Key,
                Email = hidrogenian.Email,
                UserName = hidrogenian.UserName,
                FullName = profile.GivenName + ' ' + profile.FamilyName,
                Avatar = avatarInfo.Thumbnail?.FileUrl,
                ExpirationTime = expirationTime
            };

            return new KeyValuePair<bool, AuthenticatedUser>(true, authUser);
        }

        public async Task<KeyValuePair<bool, AuthenticatedUser>> AuthenticateWithCookie(CookieAuthenticationVM cookie) {
            _logger.LogInformation("AuthenticationService.AuthenticateWithCookie - Service starts.");

            Hidrogenian dbHidrogenian = null;
            var role = string.Empty;
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
                _logger.LogError("AuthenticationService.AuthenticateWithCookie - Error: " + e.ToString());
                return new KeyValuePair<bool, AuthenticatedUser>(false, null);
            }

            var unixTimeStamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            var authToken = GenerateHashedPasswordAndSalt(dbHidrogenian.Id + dbHidrogenian.Email + unixTimeStamp);

            var profile = await _dbContext.HidroProfile.FirstOrDefaultAsync(p => p.HidrogenianId == dbHidrogenian.Id);
            var expirationTime = ((DateTimeOffset)DateTime.UtcNow.AddSeconds(
                                    cookie.TrustedAuth == "True" ? HidroConstants.TRUSTED_AUTH_EXPIRATION_TIME : HidroConstants.INTRUSTED_AUTH_EXPIRATION_TIME
                                 )).ToUnixTimeSeconds();

            var avatarInfo = profile.ProcessAvatarInfo();

            var authUser = new AuthenticatedUser {
                UserId = dbHidrogenian.Id,
                Role = role,
                AuthToken = authToken.Key,
                Email = dbHidrogenian.Email,
                FullName = profile.GivenName + ' ' + profile.FamilyName,
                Avatar = avatarInfo.Thumbnail?.FileUrl,
                ExpirationTime = expirationTime
            };

            return new KeyValuePair<bool, AuthenticatedUser>(true, authUser);
        }

        public async Task<HidroPermissionVM> ComputeAuthorizationFor(int hidrogenianId) {
            _logger.LogInformation("AuthenticationService.ComputeAuthorizationFor - Service starts.");

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

            var timestamp = DateTime.UtcNow;
            var unixTimestamp = ((DateTimeOffset)timestamp).ToUnixTimeSeconds();

            var cookieAuthToken = auth.AuthToken + auth.Role + unixTimestamp.ToString();
            var hashResult = GenerateHashedPasswordAndSalt(cookieAuthToken);

            var dbHidrogenian = await _dbContext.Hidrogenian.FindAsync(auth.UserId);
            dbHidrogenian.CookieToken = hashResult.Key;
            dbHidrogenian.CookieSetOn = timestamp;

            _dbContext.Hidrogenian.Update(dbHidrogenian);
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("AuthenticationService.GenerateCookieAuthData - Error: " + e.ToString());
                return null;
            }

            return new CookieAuthenticationVM {
                CookieToken = dbHidrogenian.CookieToken,
                TimeStamp = unixTimestamp
            };
        }

        public KeyValuePair<string, string> GenerateHashedPasswordAndSalt(string plainText) {
            _logger.LogInformation("AuthenticationService.GenerateHashedPasswordAndSalt - Service starts.");

            var salt = BCryptHelper.GenerateSalt();
            var hashedPassword = BCryptHelper.HashPassword(plainText, salt);

            return new KeyValuePair<string, string>(hashedPassword, salt);
        }

        public string GenerateRandomToken() {
            _logger.LogInformation("AuthenticationService.GenerateRandomToken - Service starts.");
            return BCryptHelper.GenerateSalt(18, SaltRevision.Revision2A);
        }

        public async Task<bool?> IsEmailAddressAvailable(string email) {
            _logger.LogInformation("AuthenticationService.IsEmailAddressAvailable - Service starts.");

            var available = true;
            try {
                available = !(await _dbContext.Hidrogenian.AnyAsync(h => h.Email == email));
            } catch (Exception e) {
                _logger.LogError("AuthenticationService.IsEmailAddressAvailable - Error: " + e.ToString());
                return null;
            }

            return available;
        }

        public async Task<bool?> IsUserNameAvailable(string username) {
            _logger.LogInformation("AuthenticationService.IsUserNameAvailable - Service starts.");

            var available = true;
            try {
                available = !(await _dbContext.Hidrogenian.AnyAsync(h => h.UserName.ToLower() == username.ToLower()));
            } catch (Exception e) {
                _logger.LogError("AuthenticationService.IsUserNameAvailable - Error: " + e.ToString());
                return null;
            }

            return available;
        }

        public async Task<KeyValuePair<bool, bool?>?> ReplaceAccountPassword(RegistrationVM recovery) {
            _logger.LogInformation("AuthenticationService.ReplaceAccountPassword - Service starts.");

            var hidrogenian = await _dbContext.Hidrogenian.FirstOrDefaultAsync(
                h => h.Email == recovery.Email && !h.EmailConfirmed && h.RecoveryToken != null && h.TokenSetOn != null
            );

            if (hidrogenian == null) return null;

            if (hidrogenian.RecoveryToken == recovery.RecoveryToken &&
                hidrogenian.TokenSetOn.Value.AddMinutes(30) > DateTime.UtcNow) {
                if (!BCryptHelper.CheckPassword(recovery.TempPassword, hidrogenian.PasswordHash))
                    return new KeyValuePair<bool, bool?>(true, null);

                var hashResult = GenerateHashedPasswordAndSalt(recovery.Password);
                hidrogenian.PasswordHash = hashResult.Key;
                hidrogenian.PasswordSalt = hashResult.Value;

                hidrogenian.EmailConfirmed = true;
                hidrogenian.RecoveryToken = null;
                hidrogenian.TokenSetOn = null;

                _dbContext.Hidrogenian.Update(hidrogenian);
                try {
                    await _dbContext.SaveChangesAsync();
                } catch (Exception e) {
                    _logger.LogError("AuthenticationService.ReplaceAccountPassword - Error: " + e.ToString());
                    return new KeyValuePair<bool, bool?>(true, false);
                }

                return new KeyValuePair<bool, bool?>(true, true);
            }

            return new KeyValuePair<bool, bool?>(false, null);
        }

        public async Task<KeyValuePair<string, string>> SetTempPasswordAndRecoveryToken(RecoveryVM recoveree) {
            _logger.LogInformation("AuthenticationService.SetTempPasswordAndRecoveryToken - Service starts.");

            var dbHidrogenian = !recoveree.Reattempt ? await _dbContext.Hidrogenian.FirstOrDefaultAsync(
                                                            h => h.Email == recoveree.Email && h.EmailConfirmed && h.DeactivatedOn == null)
                                                     : await _dbContext.Hidrogenian.FirstOrDefaultAsync(
                                                            h => h.Email == recoveree.Email && !h.EmailConfirmed && h.DeactivatedOn == null &&
                                                            h.RecoveryToken != null && h.TokenSetOn != null);

            if (dbHidrogenian == null) return new KeyValuePair<string, string>(null, null);

            var tempPassword = HelperProviders.GenerateTemporaryPassword(15);
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
                _logger.LogError("AuthenticationService.SetTempPasswordAndRecoveryToken - Error: " + e.ToString());
                return new KeyValuePair<string, string>(string.Empty, null);
            }

            return new KeyValuePair<string, string>(tempPassword, recoveryToken);
        }

        public async Task<bool?> VerifyAccountPasswordFor(int hidrogenianId, string password) {
            _logger.LogInformation("AuthenticationService.VerifyAccountPasswordFor - Service starts.");

            var account = await _dbContext.Hidrogenian.FindAsync(hidrogenianId);
            if (account == null) return null;

            return BCryptHelper.CheckPassword(password, account.PasswordHash);
        }
    }
}
