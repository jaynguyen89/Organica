using System.Threading.Tasks;
using Hidrogen.Models;

namespace Hidrogen.Services.Interfaces {
    
    public interface ITraderService {

        Task<bool> CreateInitialTraderAccountIfNecessary(int hidrogenianId);
    }
}