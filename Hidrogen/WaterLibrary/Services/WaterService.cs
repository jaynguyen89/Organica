using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WaterLibrary.DbContexts;
using WaterLibrary.Interfaces;
using WaterLibrary.Models;
using WaterLibrary.ViewModels;

namespace WaterLibrary.Services {

    public class WaterService : IWaterService {

        private readonly ILogger<WaterService> _logger;
        private readonly WaterDbContext _dbContext;

        public Task<bool> CleanEmptyFoldersInside(string folderName = "") {
            throw new NotImplementedException();
        }

        public Task<bool> CleanUserData(int hidrogenianId) {
            throw new NotImplementedException();
        }

        public async Task<bool?> DeleteOldApiTokens() {
            _logger.LogInformation("WaterService.DeleteOldApiTokens - Service starts.");

            var oldTokens = await _dbContext.Tokens.Where(t => t.TimeStamp.AddDays(14) < DateTime.UtcNow).Select(t => t).ToListAsync();

            if (oldTokens.Count != 0) {
                _dbContext.Tokens.RemoveRange(oldTokens);

                try {
                    await _dbContext.SaveChangesAsync();
                } catch (Exception e) {
                    _logger.LogError("WaterService.DeleteOldApiTokens - Error: " + e.ToString());
                    return false;
                }

                return true;
            }

            return true;
        }

        public Task<bool> DeleteUserAlbum(int hidrogenianId, string galleryFolderName = "", string attachmentFolderName = "") {
            throw new System.NotImplementedException();
        }

        public async Task<bool> SetApiToken(TokenVM token) {
            _logger.LogInformation("WaterService.SetApiToken - Service starts.");

            var dbToken = new Tokens {
                TokenString = token.Token,
                Life = token.Duration,
                Target = token.Target
            };

            _dbContext.Tokens.Add(dbToken);
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("WaterService.SetApiToken - Error: " + e.ToString());
                return false;
            }

            return true;
        }
    }
}
