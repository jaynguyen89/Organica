using System;
using System.Threading.Tasks;
using HelperLibrary;
using Hidrogen.Controllers;
using Hidrogen.DbContexts;
using Hidrogen.Models;
using Hidrogen.Services.Interfaces;
using MethaneLibrary.Interfaces;
using MethaneLibrary.Models;
using Microsoft.Extensions.Logging;

namespace Hidrogen.Services.DatabaseServices {

    public class RoleClaimerService : IRoleClaimerService {

        private readonly ILogger<RoleClaimerService> _logger;
        private readonly IRuntimeLogService _runtimeLogger;
        private readonly HidrogenDbContext _dbContext;

        public RoleClaimerService(
            ILogger<RoleClaimerService> logger,
            IRuntimeLogService runtimeLogger,
            HidrogenDbContext dbContext
        ) {
            _logger = logger;
            _runtimeLogger = runtimeLogger;
            _dbContext = dbContext;
        }

        public async Task<bool> SetRoleOnRegistrationFor(int hidrogenianId) {
            _logger.LogInformation("RoleClaimerService.SetRoleOnRegistrationFor - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(RoleClaimerService),
                Action = nameof(SetRoleOnRegistrationFor),
                Briefing = "Query database to set all default roles for a newly created account.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

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
                _logger.LogError("RoleClaimerService.SetRoleOnRegistrationFor - Error: " + e);
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(RoleClaimerService),
                    Action = nameof(SetRoleOnRegistrationFor),
                    Briefing = "Exception occurred while saving roles: " + e,
                    Severity = HidroEnums.LOGGING.ERROR.GetValue()
                });
                
                return false;
            }

            return true;
        }
    }
}
