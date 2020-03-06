using Hidrogen.Models;
using Hidrogen.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCrypt;
using Microsoft.EntityFrameworkCore;
using System;

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

        public KeyValuePair<string, string> GenerateHashedPasswordAndSalt(string plainText) {
            _logger.LogInformation("AuthenticationService.GenerateHashedPasswordAndSalt - Service starts.");

            var salt = BCryptHelper.GenerateSalt();
            var hashedPassword = BCryptHelper.HashPassword(plainText, salt);

            return new KeyValuePair<string, string>(hashedPassword, salt);
        }

        public string GenerateRandomToken() {
            _logger.LogInformation("AuthenticationService.GenerateRandomToken - Service starts.");
            return BCryptHelper.GenerateSalt(18, SaltRevision.Revision2A);
        }

        public async Task<bool?> IsEmailAddressAvailable(string email) {
            _logger.LogInformation("HidrogenianService.IsEmailAddressAvailable - Service starts.");

            var available = true;
            try {
                available = !(await _dbContext.Hidrogenian.AnyAsync(h => h.Email == email));
            } catch (Exception e) {
                _logger.LogError("HidrogenianService.IsEmailAddressAvailable - Error: " + e.ToString());
                return null;
            }

            return available;
        }

        public async Task<bool?> IsUserNameAvailable(string username) {
            _logger.LogInformation("HidrogenianService.IsUserNameAvailable - Service starts.");

            var available = true;
            try {
                available = !(await _dbContext.Hidrogenian.AnyAsync(h => h.UserName.ToLower() == username.ToLower()));
            } catch (Exception e) {
                _logger.LogError("HidrogenianService.IsUserNameAvailable - Error: " + e.ToString());
                return null;
            }

            return available;
        }
    }
}
