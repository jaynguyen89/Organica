using Hidrogen.ViewModels;
using System.Threading.Tasks;

namespace Hidrogen.Services.Interfaces {

    public interface IHidroProfileService {

        Task<bool> InsertProfileForNewlyCreatedHidrogenian(HidroProfileVM profile);
    }
}
