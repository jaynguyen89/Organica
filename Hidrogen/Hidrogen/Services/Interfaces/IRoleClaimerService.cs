using Hidrogen.ViewModels.Authorization;
using System.Threading.Tasks;

namespace Hidrogen.Services.Interfaces {

    public interface IRoleClaimerService {

        Task<bool> SetRoleOnRegistrationFor(int hidrogenianId);
    }
}
