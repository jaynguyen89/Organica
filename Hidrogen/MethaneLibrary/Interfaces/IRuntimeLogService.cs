using System.Collections.Generic;
using System.Threading.Tasks;
using MethaneLibrary.Models;
using MethaneLibrary.ViewModels;

namespace MethaneLibrary.Interfaces {
    
    public interface IRuntimeLogService {

        Task<bool> InsertRuntimeLog(RuntimeLog log);

        Task<bool> InsertRuntimeLogRange(RuntimeLog[] logs);

        Task<List<RuntimeLog>> GetPaginatedRuntimeLogs(int from, int quantity);

        Task<List<RuntimeLog>> FilterRuntimeLogs(RuntimeLogFilter filter);
    }
}