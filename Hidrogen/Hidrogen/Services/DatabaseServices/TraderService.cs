using System;
using System.Threading.Tasks;
using HelperLibrary;
using Hidrogen.DbContexts;
using Hidrogen.Models;
using Hidrogen.Services.Interfaces;
using MethaneLibrary.Interfaces;
using MethaneLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hidrogen.Services.DatabaseServices {
    
    public class TraderService : ITraderService {
        
        private readonly ILogger<TraderService> _logger;
        private readonly IRuntimeLogService _runtimeLogger;
        private readonly HidrogenDbContext _dbContext;

        public TraderService(
            ILogger<TraderService> logger,
            IRuntimeLogService runtimeLogger,
            HidrogenDbContext dbContext
        ) {
            _logger = logger;
            _runtimeLogger = runtimeLogger;
            _dbContext = dbContext;
        }

        public async Task<bool> CreateInitialTraderAccountIfNecessary(int hidrogenianId) {
            _logger.LogInformation("TraderService.CreateInitialTraderAccountIfNecessary - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(TraderService),
                Action = nameof(CreateInitialTraderAccountIfNecessary),
                Data = "HidrogenianId=" + hidrogenianId,
                Briefing = "Create initial Trader account when user activates account.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });

            if (await _dbContext.HidroTrader.AnyAsync(t => t.HidrogenianId == hidrogenianId))
                return true;
            
            var dbTrader = new HidroTrader { HidrogenianId = hidrogenianId };
            await _dbContext.HidroTrader.AddAsync(dbTrader);
            
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("TraderService.CreateInitialTraderAccountIfNecessary - Error: " + e);
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(TraderService),
                    Action = nameof(CreateInitialTraderAccountIfNecessary),
                    Briefing = "Exception occurred while inserting data: " + e,
                    Severity = HidroEnums.LOGGING.ERROR.GetValue()
                });

                return false;
            }

            return true;
        }
    }
}