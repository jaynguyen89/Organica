using Hidrogen.DbContexts;
using Hidrogen.Models;
using Hidrogen.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Hidrogen.Services.DatabaseServices {

    public class RoleClaimerService : IRoleClaimerService {

        private readonly ILogger<RoleClaimerService> _logger;
        private HidrogenDbContext _dbContext;

        public RoleClaimerService(
            ILogger<RoleClaimerService> logger,
            HidrogenDbContext dbContext
        ) {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<bool> SetRoleOnRegistrationFor(int hidrogenianId) {
            _logger.LogInformation("RoleClaimerService.SetRoleOnRegistrationFor - Servce starts.");

            var dbRoleClaim = new RoleClaimer {
                RoleId = 1, //Customer by default
                HidrogenianId = hidrogenianId,
                AllowCreate = true,
                AllowView = true,
                AllowEditOwn = true,
                AllowEditOthers = false,
                AllowDeleteOwn = true,
                AllowDeleteOthers = false,
                AllowReviveOwn = true,
                AllowReviveOthers = false
            };

            _dbContext.RoleClaimer.Add(dbRoleClaim);
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("RoleClaimerService.SetRoleOnRegistrationFor - Error: " + e.ToString());
                return false;
            }

            return true;
        }
    }
}
