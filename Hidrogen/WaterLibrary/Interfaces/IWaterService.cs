using System.Threading.Tasks;
using WaterLibrary.ViewModels;

namespace WaterLibrary.Interfaces {

    public interface IWaterService {

        /// <summary>
        /// Insert an API Token to database for a REST request in Carbon project.
        /// </summary>
        Task<bool> SetApiToken(TokenVM token);

        /// <summary>
        /// Delete all API Tokens that are expired.
        /// </summary>
        Task<bool?> DeleteOldApiTokens();

        /// <summary>
        /// Param folderName == "" to clean from the WWW_ROOT folder, otherwise,
        /// supply a folder name to start cleaning from the specified folder.
        /// </summary>
        Task<bool> CleanEmptyFoldersInside(string folderName = "");

        /// <summary>
        /// Delete all data associating with hidrogenianId on Water Server. Useful for deleting an account.
        /// </summary>
        Task<bool> CleanUserData(int hidrogenianId);

        /// <summary>
        /// Useful when user deletes an article or close a thread. One of the 2 string arguments must be empty.
        /// </summary>
        Task<bool> DeleteUserAlbum(int hidrogenianId, string galleryFolderName = "", string attachmentFolderName = "");

        /// <summary>
        /// Send HttpClient POST request to Water API. Returns null on error, otherwise, an instance of AvatarResultVM.
        /// </summary>
        Task<AvatarResultVM> SendSaveAvatarRequestToWater(AssetFormVM uploading);
        
        /// <summary>
        /// Send HttpClient DELETE request to Water API. Returns null on error, otherwise, an instance of AvatarResultVM.
        /// </summary>
        Task<AvatarResultVM> SendDeleteAvatarRequestToWater(string apiKey, string photoName);
        
        /// <summary>
        /// Send HttpClient POST request to Water API. Returns null on error, otherwise, an instance of AvatarResultVM.
        /// </summary>
        Task<AvatarResultVM> SendReplaceAvatarRequestToWater(AssetReplaceVM uploading);
    }
}
