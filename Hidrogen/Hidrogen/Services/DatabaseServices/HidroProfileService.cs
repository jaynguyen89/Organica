using Hidrogen.Models;
using Hidrogen.Services.Interfaces;
using Hidrogen.ViewModels;
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
    }
}
