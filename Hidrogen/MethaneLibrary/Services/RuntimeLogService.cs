using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MethaneLibrary.DbContext;
using MethaneLibrary.Interfaces;
using MethaneLibrary.Models;
using MethaneLibrary.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MethaneLibrary.Services {
    
    public class RuntimeLogService : IRuntimeLogService {

        private readonly ILogger<RuntimeLogService> _logger;
        private readonly MethaneDbContext _dbContext;

        public RuntimeLogService(
            ILogger<RuntimeLogService> logger,
            IOptions<ServerOptions> options
        ) {
            _logger = logger;
            _dbContext = new MethaneDbContext(options);
        }

        public async Task<bool> InsertRuntimeLog(RuntimeLog log) {
            _logger.LogInformation("RuntimeLogService.InsertRuntimeLog - Service starts.");

            try {
                //await _dbContext.RuntimeLog.InsertOneAsync(log);
            } catch (Exception e) {
                _logger.LogError("RuntimeLogService.InsertRuntimeLog - Error: " + e);
                return false;
            }
            
            _logger.LogInformation("RuntimeLogService.InsertRuntimeLog - Service done.");
            return true;
        }

        public async Task<bool> InsertRuntimeLogRange(RuntimeLog[] logs) {
            _logger.LogInformation("RuntimeLogService.InsertRuntimeLogRange - Service starts.");

            try {
                //await _dbContext.RuntimeLog.InsertManyAsync(logs);
            } catch (Exception e) {
                _logger.LogError("RuntimeLogService.InsertRuntimeLogRange - Error: " + e);
                return false;
            }
            
            _logger.LogInformation("RuntimeLogService.InsertRuntimeLogRange - Service done.");
            return true;
        }

        public async Task<List<RuntimeLog>> GetPaginatedRuntimeLogs(int from = 0, int quantity = 100) {
            _logger.LogInformation("RuntimeLogService.GetPaginatedRuntimeLogs - from=" + from + " quantity=" + quantity);

            List<RuntimeLog> logs;
            try {
                //logs = await _dbContext.RuntimeLog.Find(Builders<RuntimeLog>.Filter.Empty).Skip(from).Limit(quantity).ToListAsync();
            } catch (Exception e) {
                _logger.LogError("RuntimeLogService.InsertRuntimeLogRange - Error: " + e);
                return null;
            }

            _logger.LogInformation("RuntimeLogService.GetPaginatedRuntimeLogs - Service done.");
            return null; //logs;
        }

        public Task<List<RuntimeLog>> FilterRuntimeLogs(RuntimeLogFilter filter) {
            throw new NotImplementedException();
        }
    }
}