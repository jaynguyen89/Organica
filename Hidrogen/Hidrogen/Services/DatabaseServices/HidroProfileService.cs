using System;
using System.Threading.Tasks;
using Hidrogen.DbContexts;
using Hidrogen.Models;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

        public async Task<string> DeleteAvatarInformation(int hidrogenianId) {
            _logger.LogInformation("HidroProfileService.GetPublicProfileFor - profileId=" + hidrogenianId);

            var dbProfile = await _dbContext.HidroProfile.FirstOrDefaultAsync(p => p.HidrogenianId == hidrogenianId);
            if (dbProfile == null) return null;

            var avatarInfo = dbProfile.AvatarInformation;
            dbProfile.AvatarInformation = null;
            _dbContext.HidroProfile.Update(dbProfile);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidroProfileService.GetPublicProfileFor - Error: " + e);
                return string.Empty;
            }

            return avatarInfo;
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
                _logger.LogError("HidroProfileService.InsertProfileForNewlyCreatedHidrogenian - Error: " + e);
                return false;
            }

            return true;
        }

        public async Task<bool?> UpdateHidrogenianAvatar(HidroProfileVM profile) {
            _logger.LogInformation("HidroProfileService.UpdateHidrogenianAvatar - Service starts.");

            HidroProfile dbProfile;
            if (profile.Id != 0) dbProfile = await _dbContext.HidroProfile.FindAsync(profile.Id);
            else dbProfile = await _dbContext.HidroProfile.FirstOrDefaultAsync(p => p.HidrogenianId == profile.HidrogenianId);
            if (dbProfile == null) return null;

            dbProfile.AvatarInformation = JsonConvert.SerializeObject(profile.Avatar);
            _dbContext.HidroProfile.Update(dbProfile);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidroProfileService.UpdateHidrogenianAvatar - Error: " + e);
                return false;
            }

            return true;
        }

        public async Task<bool?> UpdatePrivateProfile(HidroProfileVM profile) {
            _logger.LogInformation("HidroProfileService.UpdatePrivateProfile - Service starts.");

            var dbProfile = await _dbContext.HidroProfile.FindAsync(profile.Id);
            if (dbProfile == null) return null;

            dbProfile.FamilyName = profile.FamilyName;
            dbProfile.GivenName = profile.GivenName;
            dbProfile.Gender = profile.Gender == 0 ? (bool?) null : (profile.Gender == 1);
            dbProfile.DateOfBith = profile.Birthday.Birth;
            dbProfile.Ethnicity = profile.Ethnicity;
            dbProfile.Company = profile.Company;
            dbProfile.JobTitle = profile.JobTitle;
            dbProfile.PersonalWebsite = profile.Website;
            dbProfile.SelfIntroduction = profile.SelfIntroduction;
            
            _dbContext.HidroProfile.Update(dbProfile);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidroProfileService.UpdatePrivateProfile - Error: " + e);
                return false;
            }

            return true;
        }
    }
}
