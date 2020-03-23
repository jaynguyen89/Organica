using Hidrogen.DbContexts;
using Hidrogen.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Hidrogen.Services.DatabaseServices {

    public class AccountService : IAccountService {

        private readonly ILogger<AccountService> _logger;
        private HidrogenDbContext _dbContext;

        public AccountService(
            ILogger<AccountService> logger,
            HidrogenDbContext dbContext
        ) {
            _logger = logger;
            _dbContext = dbContext;
        }
    }
}
