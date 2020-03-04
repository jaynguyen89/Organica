using Hidrogen.Models;
using Hidrogen.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Hidrogen.Services.DatabaseServices {

    public class AuthenticationService : IAuthenticationService {

        private readonly ILogger<AuthenticationService> _logger;
        private HidrogenDbContext _dbContext;

        public AuthenticationService(
            ILogger<AuthenticationService> logger,
            HidrogenDbContext dbContext
        ) {
            _logger = logger;
            _dbContext = dbContext;
        }


    }
}
