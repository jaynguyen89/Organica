using System.Threading.Tasks;
using Hidrogen.ViewModels;

namespace Hidrogen.Services.Interfaces {

    public interface IHidroProfileService {

        /// <summary>
        /// Returns true indicating successful database process, otherwise, returns false.
        /// </summary>
        Task<bool> InsertProfileForNewlyCreatedHidrogenian(HidroProfileVM profile);

        /// <summary>
        /// Returns null indicating profile not found with the given hidrogenian ID, otherwise, returns the profile. 
        /// </summary>
        Task<HidroProfileVM> GetPrivateProfileFor(int hidrogenianId);
        
        /// <summary>
        /// Returns null indicating profile not found with the given hidrogenian EMAIL, otherwise, returns the profile. 
        /// </summary>
        Task<HidroProfileVM> GetPrivateProfileByEmail(string email);

        /// <summary>
        /// Returns null indicating profile not found with the given data, returns true indicating successful update, otherwise returns false.
        /// </summary>
        Task<bool?> UpdateHidrogenianAvatar(HidroProfileVM profile);

        /// <summary>
        /// Returns null indicating profile not found with the given data, returns true indicating successful deletion, otherwise returns false.
        /// </summary>
        Task<string> DeleteAvatarInformation(int hidrogenianId);

        /// <summary>
        /// Returns null indicating profile not found with the given data, returns true indicating successful updating, otherwise returns false.
        /// </summary>
        Task<bool?> UpdatePrivateProfile(HidroProfileVM profile);
    }
}
