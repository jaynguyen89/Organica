using System;
using System.Linq;
using System.Threading.Tasks;
using HelperLibrary;
using Hidrogen.Controllers;
using Hidrogen.DbContexts;
using Hidrogen.Models;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels;
using MethaneLibrary.Interfaces;
using MethaneLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Hidrogen.Services.DatabaseServices {

    public class HidroProfileService : IHidroProfileService {

        private readonly ILogger<HidroProfileService> _logger;
        private readonly IRuntimeLogService _runtimeLogger;
        private readonly HidrogenDbContext _dbContext;

        public HidroProfileService(
            ILogger<HidroProfileService> logger,
            IRuntimeLogService runtimeLogger,
            HidrogenDbContext dbContext
        ) {
            _logger = logger;
            _runtimeLogger = runtimeLogger;
            _dbContext = dbContext;
        }

        public async Task<string> DeleteAvatarInformation(int hidrogenianId) {
            _logger.LogInformation("HidroProfileService.GetPrivateProfileFor - profileId=" + hidrogenianId);
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(HidroProfileService),
                Action = nameof(DeleteAvatarInformation),
                Briefing = "Query database to remove avatar data from profile having hidrogenianId = " + hidrogenianId,
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var dbProfile = await _dbContext.HidroProfile.FirstOrDefaultAsync(p => p.HidrogenianId == hidrogenianId);
            if (dbProfile == null) return null;

            var avatarInfo = dbProfile.AvatarInformation;
            dbProfile.AvatarInformation = null;
            _dbContext.HidroProfile.Update(dbProfile);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("HidroProfileService.GetPrivateProfileFor - Error: " + e);
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(HidroProfileService),
                    Action = nameof(DeleteAvatarInformation),
                    Briefing = "Exception occurred while saving changes: " + e,
                    Severity = HidroEnums.LOGGING.ERROR.GetValue()
                });
                
                return string.Empty;
            }

            return avatarInfo;
        }

        public async Task<HidroProfileVM> GetPrivateProfileFor(int hidrogenianId) {
            _logger.LogInformation("HidroProfileService.GetPrivateProfileFor - hidrogenianId=" + hidrogenianId);
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(HidroProfileService),
                Action = nameof(GetPrivateProfileFor),
                Briefing = "Query database to get public profile details having hidrogenianId = " + hidrogenianId,
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var dbProfile = await _dbContext.HidroProfile.FirstOrDefaultAsync(p => p.HidrogenianId == hidrogenianId);
            return dbProfile;
        }

        public async Task<HidroProfileVM> GetPrivateProfileByEmail(string email) {
            _logger.LogInformation("HidroProfileService.GetPrivateProfileFor - email=" + email);
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(HidroProfileService),
                Action = nameof(GetPrivateProfileByEmail),
                Briefing = "Query database to get public profile details having email = " + email,
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var dbProfile = await (from p in _dbContext.HidroProfile
                                   join h in _dbContext.Hidrogenian
                                       on p.HidrogenianId equals h.Id
                                   where h.Email == email
                                       select p).FirstOrDefaultAsync();
            
            return dbProfile;
        }

        public async Task<bool> InsertProfileForNewlyCreatedHidrogenian(HidroProfileVM profile) {
            _logger.LogInformation("HidroProfileService.InsertProfileForNewlyCreatedHidrogenian - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(HidroProfileService),
                Action = nameof(InsertProfileForNewlyCreatedHidrogenian),
                Data = JsonConvert.SerializeObject(profile),
                Briefing = "Query database to save an initial profile for a newly created account.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

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
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(HidroProfileService),
                    Action = nameof(InsertProfileForNewlyCreatedHidrogenian),
                    Briefing = "Exception occurred while saving profile: " + e,
                    Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
                });
                
                return false;
            }

            return true;
        }

        public async Task<bool?> UpdateHidrogenianAvatar(HidroProfileVM profile) {
            _logger.LogInformation("HidroProfileService.UpdateHidrogenianAvatar - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(HidroProfileService),
                Action = nameof(UpdateHidrogenianAvatar),
                Data = JsonConvert.SerializeObject(profile),
                Briefing = "Query database to update a profile by ID.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

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
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(HidroProfileService),
                    Action = nameof(UpdateHidrogenianAvatar),
                    Briefing = "Exception occurred while saving changes: " + e,
                    Severity = HidroEnums.LOGGING.ERROR.GetValue()
                });
                return false;
            }

            return true;
        }

        public async Task<bool?> UpdatePrivateProfile(HidroProfileVM profile) {
            _logger.LogInformation("HidroProfileService.UpdatePrivateProfile - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(HidroProfileService),
                Action = nameof(UpdatePrivateProfile),
                Data = JsonConvert.SerializeObject(profile),
                Briefing = "Query database to update a private profile by ID.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            var dbProfile = await _dbContext.HidroProfile.FindAsync(profile.Id);
            if (dbProfile == null) return null;

            dbProfile.FamilyName = profile.FamilyName;
            dbProfile.GivenName = profile.GivenName;
            dbProfile.Gender = profile.Gender == 0 ? (bool?) null : (profile.Gender == 1);
            dbProfile.DateOfBirth = profile.Birthday.Birth;
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
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(HidroProfileService),
                    Action = nameof(UpdatePrivateProfile),
                    Briefing = "Exception occurred while saving changes: " + e,
                    Severity = HidroEnums.LOGGING.ERROR.GetValue()
                });
                return false;
            }

            return true;
        }
    }
}
