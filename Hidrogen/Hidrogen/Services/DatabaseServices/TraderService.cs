using Hidrogen.DbContexts;
using Hidrogen.Services.Interfaces;
using MethaneLibrary.Interfaces;
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
    }
}