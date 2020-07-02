using System;
using System.Threading.Tasks;
using HelperLibrary;
using Hidrogen.DbContexts;
using Hidrogen.Models;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels;
using Hidrogen.ViewModels.Authentication;
using MethaneLibrary.Interfaces;
using MethaneLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Hidrogen.Services.DatabaseServices {

    public class HidrogenianService : HidroServiceBase, IHidrogenianService  {

        private readonly ILogger<HidrogenianService> _logger;
        private readonly IRuntimeLogService _runtimeLogger;
        private readonly HidrogenDbContext _dbContext;

        public HidrogenianService(
            ILogger<HidrogenianService> logger,
            IRuntimeLogService runtimeLogger,
            HidrogenDbContext dbContext,
            IMemoryCache memoryCache,
            IHttpContextAccessor httpContextAccessor
        ) : base(memoryCache, httpContextAccessor) {
            _logger = logger;
            _runtimeLogger = runtimeLogger;
            _dbContext = dbContext;
        }

        public async Task<HidrogenianVM> GetHidrogenianByEmail(string email) {
            _logger.LogInformation("HidrogenianService.GetHidrogenianByEmail - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(HidrogenianService),
                Action = nameof(GetHidrogenianByEmail),
                Briefing = "Query database to get an active Hidrogenian by email = " + email,
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });
            
            return await _dbContext.Hidrogenian.FirstOrDefaultAsync(
                h => h.Email == email && h.EmailConfirmed && h.DeactivatedOn == null
            );
        }

        public async Task<Hidrogenian> GetUnactivatedHidrogenianByEmail(string email) {
            _logger.LogInformation("HidrogenianService.GetUnactivatedHidrogenianByEmail - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(HidrogenianService),
                Action = nameof(GetUnactivatedHidrogenianByEmail),
                Briefing = "Query database to get an inactive Hidrogenian by email = " + email,
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var unactivatedUser = ReadFromMemoryCache<Hidrogenian>("HidrogenianService_UnactivatedUser", email);
            if (unactivatedUser != null) return unactivatedUser;
            
            unactivatedUser = await _dbContext.Hidrogenian.FirstOrDefaultAsync(
                h => h.Email == email && !h.EmailConfirmed && h.DeactivatedOn == null && h.RecoveryToken != null &&
                     h.TokenSetOn != null && h.TokenSetOn.Value.AddHours(24) > DateTime.UtcNow
            );
            
            if (unactivatedUser != null)
                InsertMemoryCacheEntry("HidrogenianService_UnactivatedUser", unactivatedUser, typeof(Hidrogenian).GetProperties().Length, email);
            
            return unactivatedUser;
        }

        public async Task<HidrogenianVM> GetUnactivatedHidrogenianVMByEmail(string email) {
            _logger.LogInformation("HidrogenianService.GetUnactivatedHidrogenianVMByEmail - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(HidrogenianService),
                Action = nameof(GetUnactivatedHidrogenianVMByEmail),
                Briefing = "Query database to get an inactive HidrogenianVM by email = " + email,
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });
            
            return await _dbContext.Hidrogenian.FirstOrDefaultAsync(
                h => h.Email == email && !h.EmailConfirmed && h.DeactivatedOn == null && h.RecoveryToken != null &&
                     h.TokenSetOn != null && h.TokenSetOn.Value.AddHours(24) > DateTime.UtcNow
            );
        }

        public async Task<HidrogenianVM> InsertNewHidrogenian(RegistrationVM registration) {
            _logger.LogInformation("HidrogenianService.InsertNewHidrogenian - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(HidrogenianService),
                Action = nameof(GetUnactivatedHidrogenianByEmail),
                Briefing = "Query database to save a newly created account for user.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var dbHidrogenian = new Hidrogenian {
                Email = registration.Email,
                UserName = registration.UserName,
                PasswordHash = registration.Password,
                PasswordSalt = registration.PasswordConfirm
            };

            await _dbContext.Hidrogenian.AddAsync(dbHidrogenian);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidrogenianService.InsertNewHidrogenian - Error: " + e);
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(HidrogenianService),
                    Action = nameof(GetUnactivatedHidrogenianByEmail),
                    Briefing = "Exception occurred while saving data: " + e,
                    Severity = HidroEnums.LOGGING.ERROR.GetValue()
                });
                
                return null;
            }

            HidrogenianVM hidrogenian = dbHidrogenian;
            hidrogenian.FamilyName = registration.FamilyName;
            hidrogenian.GivenName = registration.GivenName;

            return hidrogenian;
        }

        public async Task<bool> RemoveNewlyInsertedHidrogenian(int hidrogenianId) {
            _logger.LogInformation("HidrogenianService.RemoveNewlyInsertedHidrogenian - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(HidrogenianService),
                Action = nameof(RemoveNewlyInsertedHidrogenian),
                Briefing = "Query database to remove the just-inserted-data after a service failed (likely EmailService).",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var dbHidrogenian = await _dbContext.Hidrogenian.FindAsync(hidrogenianId);
            _dbContext.Remove(dbHidrogenian);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidrogenianService.RemoveNewlyInsertedHidrogenian - Error: " + e);
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(HidrogenianService),
                    Action = nameof(RemoveNewlyInsertedHidrogenian),
                    Briefing = "Exception occurred while removing data: " + e,
                    Severity = HidroEnums.LOGGING.ERROR.GetValue()
                });
                
                return false;
            }

            return true;
        }

        public async Task<bool?> RemoveTwoFaSecretKeyFor(int hidrogenianId) {
            _logger.LogInformation("HidrogenianService.RemoveTwoFaSecretKeyFor - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(HidrogenianService),
                Action = nameof(RemoveTwoFaSecretKeyFor),
                Briefing = "Query database to remove 2FA data for an account.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var account = await _dbContext.Hidrogenian.FindAsync(hidrogenianId);
            if (account == null) return null;

            account.TwoFaSecretKey = null;
            account.TwoFactorEnabled = false;
            _dbContext.Hidrogenian.Update(account);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidrogenianService.RemoveTwoFaSecretKeyFor - Error: " + e);
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(HidrogenianService),
                    Action = nameof(RemoveTwoFaSecretKeyFor),
                    Briefing = "Exception occurred while removing data: " + e,
                    Severity = HidroEnums.LOGGING.ERROR.GetValue()
                });
                
                return false;
            }

            return true;
        }

        public async Task<bool?> SaveTwoFaSecretKeyFor(int hidrogenianId, string secretKey) {
            _logger.LogInformation("HidrogenianService.SaveTwoFaSecretKeyFor - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(HidrogenianService),
                Action = nameof(SaveTwoFaSecretKeyFor),
                Briefing = "Query database to save 2FA secret key for an account.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var account = await _dbContext.Hidrogenian.FindAsync(hidrogenianId);
            if (account == null) return null;

            account.TwoFaSecretKey = secretKey;
            _dbContext.Hidrogenian.Update(account);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidrogenianService.SaveTwoFaSecretKeyFor - Error: " + e);
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(HidrogenianService),
                    Action = nameof(SaveTwoFaSecretKeyFor),
                    Briefing = "Exception occurred while saving data: " + e,
                    Severity = HidroEnums.LOGGING.ERROR.GetValue()
                });
                return false;
            }

            return true;
        }

        public async Task<bool> SetAccountConfirmationToken(HidrogenianVM hidrogenian) {
            _logger.LogInformation("HidrogenianService.SetAccountConfirmationToken - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(HidrogenianService),
                Action = nameof(SetAccountConfirmationToken),
                Briefing = "Query database to set confirmation token for an account to recover.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var dbHidrogenian = await _dbContext.Hidrogenian.FindAsync(hidrogenian.Id);

            dbHidrogenian.RecoveryToken = hidrogenian.Token;
            dbHidrogenian.TokenSetOn = DateTime.UtcNow;

            _dbContext.Update(dbHidrogenian);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidrogenianService.SetAccountConfirmationToken - Error: " + e);
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(HidrogenianService),
                    Action = nameof(SetAccountConfirmationToken),
                    Briefing = "Exception occurred while saving changes: " + e,
                    Severity = HidroEnums.LOGGING.ERROR.GetValue()
                });
                
                return false;
            }

            return true;
        }
    }
}
