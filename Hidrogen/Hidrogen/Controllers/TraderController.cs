using Hidrogen.Services.Interfaces;
using MethaneLibrary.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Hidrogen.Controllers {
    
    public class TraderController : AppController {
        
        private readonly ILogger<TraderController> _logger;
        private readonly IRuntimeLogService _runtimeLogger;
        private readonly ITraderService _traderService;

        public TraderController(
            ILogger<TraderController> logger,
            IRuntimeLogService runtimeLogger,
            ITraderService traderService,
            IDistributedCache redisCache
        ) : base(redisCache) {
            _logger = logger;
            _runtimeLogger = runtimeLogger;
            _traderService = traderService;
        }
    }
}