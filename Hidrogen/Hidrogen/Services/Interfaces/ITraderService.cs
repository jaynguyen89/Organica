using System.Threading.Tasks;

namespace Hidrogen.Services.Interfaces {
    
    public interface ITraderService {

        Task<bool> CreateInitialTraderAccount(int hidrogenianId);
    }
}