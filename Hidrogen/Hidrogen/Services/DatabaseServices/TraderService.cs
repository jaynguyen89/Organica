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

        public async Task<bool> CreateInitialTraderAccount(int hidrogenianId) {
            _logger.LogInformation("TraderService.CreateInitialTraderAccount - Service starts.");
            await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                Controller = nameof(TraderService),
                Action = nameof(CreateInitialTraderAccount),
                Data = hidrogenianId.ToString(),
                Briefing = "Create initial Trader account when user activates account.",
                Severity = HidroEnums.LOGGING.INFORMATION.GetValue()
            });
            
            var dbTrader = new HidroTrader { HidrogenianId = hidrogenianId };
            await _dbContext.HidroTrader.AddAsync(dbTrader);
            
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("TraderService.CreateInitialTraderAccount - Error: " + e);
                await _runtimeLogger.InsertRuntimeLog(new RuntimeLog {
                    Controller = nameof(TraderService),
                    Action = nameof(CreateInitialTraderAccount),
                    Briefing = "Exception occurred while inserting data: " + e,
                    Severity = HidroEnums.LOGGING.ERROR.GetValue()
                });

                return false;
            }

            return true;
        }
    }
}