using Hidrogen.DbContexts;
using Hidrogen.Models;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels;
using Hidrogen.ViewModels.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Hidrogen.Services.DatabaseServices {

    public class HidrogenianService : IHidrogenianService {

        private readonly ILogger<HidrogenianService> _logger;
        private HidrogenDbContext _dbContext;

        public HidrogenianService(
            ILogger<HidrogenianService> logger,
            HidrogenDbContext dbContext
        ) {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<HidrogenianVM> GetHidrogenianByEmail(string email) {
            _logger.LogInformation("HidrogenianService.GetHidrogenianByEmail - Service starts.");
            return await _dbContext.Hidrogenian.FirstOrDefaultAsync(
                h => h.Email == email && h.EmailConfirmed && h.DeactivatedOn == null
            );
        }

        public async Task<HidrogenianVM> GetUnactivatedHidrogenianByEmail(string email) {
            _logger.LogInformation("HidrogenianService.GetUnactivatedHidrogenianByEmail - Service starts.");
            return await _dbContext.Hidrogenian.FirstOrDefaultAsync(
                h => h.Email == email && !h.EmailConfirmed && h.DeactivatedOn == null && h.RecoveryToken != null &&
                     h.TokenSetOn != null && h.TokenSetOn.Value.AddHours(24) > DateTime.UtcNow
            );
        }

        public async Task<HidrogenianVM> InsertNewHidrogenian(RegistrationVM registration) {
            _logger.LogInformation("HidrogenianService.InserNewHidrogenian - Service starts.");

            var dbHidrogenian = new Hidrogenian {
                Email = registration.Email,
                UserName = registration.UserName,
                PasswordHash = registration.Password,
                PasswordSalt = registration.PasswordConfirm
            };

            _dbContext.Hidrogenian.Add(dbHidrogenian);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidrogenianService.InserNewHidrogenian - Error: " + e.ToString());
                return null;
            }

            HidrogenianVM hidrogenian = dbHidrogenian;
            hidrogenian.FamilyName = registration.FamilyName;
            hidrogenian.GivenName = registration.GivenName;

            return hidrogenian;
        }

        public async Task<bool> RemoveNewlyInsertedHidrogenian(int hidrogenianId) {
            _logger.LogInformation("HidrogenianService.RemoveNewlyInsertedHidrogenian - Service starts.");

            var dbHidrogenian = await _dbContext.Hidrogenian.FindAsync(hidrogenianId);
            _dbContext.Remove(dbHidrogenian);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidrogenianService.RemoveNewlyInsertedHidrogenian - Error: " + e.ToString());
                return false;
            }

            return true;
        }

        public async Task<bool?> RemoveTwoFaSecretKeyFor(int hidrogenianId) {
            _logger.LogInformation("HidrogenianService.RemoveTwoFaSecretKeyFor - Service starts.");

            var account = await _dbContext.Hidrogenian.FindAsync(hidrogenianId);
            if (account == null) return null;

            account.TwoFaSecretKey = null;
            account.TwoFactorEnabled = false;
            _dbContext.Hidrogenian.Update(account);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidrogenianService.RemoveTwoFaSecretKeyFor - Error: " + e.ToString());
                return false;
            }

            return true;
        }

        public async Task<bool?> SaveTwoFaSecretKeyFor(int hidrogenianId, string secretKey) {
            _logger.LogInformation("HidrogenianService.SaveTwoFaSecretKeyFor - Service starts.");

            var account = await _dbContext.Hidrogenian.FindAsync(hidrogenianId);
            if (account == null) return null;

            account.TwoFaSecretKey = secretKey;
            _dbContext.Hidrogenian.Update(account);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidrogenianService.SaveTwoFaSecretKeyFor - Error: " + e.ToString());
                return false;
            }

            return true;
        }

        public async Task<bool> SetAccountConfirmationToken(HidrogenianVM hidrogenian) {
            _logger.LogInformation("HidrogenianService.SetAccountConfirmationToken - Service starts.");

            var dbHidrogenian = await _dbContext.Hidrogenian.FindAsync(hidrogenian.Id);

            dbHidrogenian.RecoveryToken = hidrogenian.Token;
            dbHidrogenian.TokenSetOn = DateTime.UtcNow;

            _dbContext.Update(dbHidrogenian);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidrogenianService.SetAccountConfirmationToken - Error: " + e.ToString());
                return false;
            }

            return true;
        }
    }
}
