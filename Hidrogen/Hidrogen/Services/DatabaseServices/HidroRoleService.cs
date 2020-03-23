using Hidrogen.DbContexts;
using Hidrogen.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Hidrogen.Services.DatabaseServices {

    public class HidroRoleService : IHidroRoleService {

        private readonly ILogger<HidroRoleService> _logger;
        private HidrogenDbContext _dbContext;

        public HidroRoleService(
            ILogger<HidroRoleService> logger,
            HidrogenDbContext dbContext
        ) {
            _logger = logger;
            _dbContext = dbContext;
        }
    }
}