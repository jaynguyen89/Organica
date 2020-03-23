using Hidrogen.ViewModels;
using System.Threading.Tasks;

namespace Hidrogen.Services.Interfaces {

    public interface IHidroProfileService {

        /// <summary>
        /// Returns true indicating successful database process, otherwise, returns false.
        /// </summary>
        Task<bool> InsertProfileForNewlyCreatedHidrogenian(HidroProfileVM profile);

        /// <summary>
        /// Returns null indicating profile not found with the given hidrogenian ID, otherwise, returns the profile. 
        /// </summary>
        Task<HidroProfileVM> GetPublicProfileFor(int hidrogenianId);

        /// <summary>
        /// Returns null indicating profile not found with the given data, returns true indicating successful update, otherwise returns false.
        /// </summary>
        Task<bool?> UpdateHidrogenianAvatar(HidroProfileVM profile);

        /// <summary>
        /// Returns null indicating profile not found with the given data, returns true indicating successful deletion, otherwise returns false.
        /// </summary>
        Task<bool?> DeleteAvatarInformation(int profileId);

        /// <summary>
        /// Returns null indicating profile not found with the given data, returns true indicating successful updating, otherwise returns false.
        /// </summary>
        Task<bool?> UpdatePublicProfile(HidroProfileVM profile);
    }
}
