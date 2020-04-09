using System.Threading.Tasks;

namespace Hidrogen.Services.Interfaces {

    public interface IRoleClaimerService {

        /// <summary>
        /// Returns true indicating successful database process, otherwise returns false.
        /// </summary>
        Task<bool> SetRoleOnRegistrationFor(int hidrogenianId);
    }
}
