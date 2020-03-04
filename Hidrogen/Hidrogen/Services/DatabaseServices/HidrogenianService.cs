using Hidrogen.Models;
using Hidrogen.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Hidrogen.Services.DatabaseServices {

    public class HidrogenianService : IHidrogenianService {

        private readonly ILogger<HidrogenianService> _logger;
        private HidrogenDbContext _dbContext;


        public HidrogenianService(
            ILogger<HidrogenianService> logger,
            HidrogenDbContext dbContext
        ) {
            _logger = logger;
            _dbContext = dbContext;
        }



    }
}
