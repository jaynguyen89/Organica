using Hidrogen.Services.Interfaces;
using MethaneLibrary.Interfaces;
using Microsoft.Extensions.Logging;
using WaterLibrary.Interfaces;

namespace Hidrogen.Controllers {
    
    public class TraderController {
        
        private readonly ILogger<TraderController> _logger;
        private readonly IRuntimeLogService _runtimeLogger;
        private readonly ITraderService _traderService;

        public TraderController(
            ILogger<TraderController> logger,
            IRuntimeLogService runtimeLogger,
            ITraderService traderService
        ) {
            _logger = logger;
            _runtimeLogger = runtimeLogger;
            _traderService = traderService;
        }
    }
}