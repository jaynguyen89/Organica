using Hidrogen.DbContexts;
using Hidrogen.Models;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Hidrogen.Services.DatabaseServices {

    public class HidroProfileService : IHidroProfileService {

        private readonly ILogger<HidroProfileService> _logger;
        private HidrogenDbContext _dbContext;

        public HidroProfileService(
            ILogger<HidroProfileService> logger,
            HidrogenDbContext dbContext
        ) {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<bool?> DeleteAvatarInformation(int profileId) {
            _logger.LogInformation("HidroProfileService.GetPublicProfileFor - profileId=" + profileId);

            var dbProfile = await _dbContext.HidroProfile.FindAsync(profileId);
            if (dbProfile == null) return null;

            dbProfile.AvatarInformation = null;
            _dbContext.HidroProfile.Update(dbProfile);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidroProfileService.GetPublicProfileFor - Error: " + e.ToString());
                return false;
            }

            return true;
        }

        public async Task<HidroProfileVM> GetPublicProfileFor(int hidrogenianId) {
            _logger.LogInformation("HidroProfileService.GetPublicProfileFor - hidrogenianId=" + hidrogenianId);

            var dbProfile = await _dbContext.HidroProfile.FirstOrDefaultAsync(p => p.HidrogenianId == hidrogenianId);
            if (dbProfile == null) return null;

            return dbProfile;
        }

        public async Task<bool> InsertProfileForNewlyCreatedHidrogenian(HidroProfileVM profile) {
            _logger.LogInformation("HidroProfileService.InsertProfileForNewlyCreatedHidrogenian - Service starts.");

            var dbProfile = new HidroProfile {
                HidrogenianId = profile.HidrogenianId,
                FamilyName = profile.FamilyName,
                GivenName = profile.GivenName
            };

            _dbContext.Add(dbProfile);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidroProfileService.InsertProfileForNewlyCreatedHidrogenian - Error: " + e.ToString());
                return false;
            }

            return true;
        }

        public async Task<bool?> UpdateHidrogenianAvatar(HidroProfileVM profile) {
            _logger.LogInformation("HidroProfileService.UpdateHidrogenianAvatar - Service starts.");

            var dbProfile = await _dbContext.HidroProfile.FindAsync(profile.Id);
            if (dbProfile == null) return null;

            dbProfile.AvatarInformation = profile.Avatar;
            _dbContext.HidroProfile.Update(dbProfile);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidroProfileService.UpdateHidrogenianAvatar - Error: " + e.ToString());
                return false;
            }

            return true;
        }

        public async Task<bool?> UpdatePublicProfile(HidroProfileVM profile) {
            _logger.LogInformation("HidroProfileService.UpdatePublicProfile - Service starts.");

            var dbProfile = await _dbContext.HidroProfile.FindAsync(profile.Id);
            if (dbProfile == null) return null;

            dbProfile = profile;
            _dbContext.HidroProfile.Update(dbProfile);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidroProfileService.UpdatePublicProfile - Error: " + e.ToString());
                return false;
            }

            return true;
        }
    }
}
