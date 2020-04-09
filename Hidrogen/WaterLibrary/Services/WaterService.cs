using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HelperLibrary.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WaterLibrary.DbContexts;
using WaterLibrary.Interfaces;
using WaterLibrary.Models;
using WaterLibrary.ViewModels;

namespace WaterLibrary.Services {

    public class WaterService : IWaterService {

        private readonly ILogger<WaterService> _logger;
        private readonly WaterDbContext _dbContext;
        private readonly HttpClient _waterRequest = new HttpClient();

        public WaterService(
            ILogger<WaterService> logger,
            WaterDbContext dbContext
        ) {
            _logger = logger;
            _dbContext = dbContext;

            _waterRequest.BaseAddress = new Uri(@"https://water.jaydeveloper.com/");
            _waterRequest.DefaultRequestHeaders.Accept.Clear();
        }

        public Task<bool> CleanEmptyFoldersInside(string folderName = "") {
            throw new NotImplementedException();
        }

        public Task<bool> CleanUserData(int hidrogenianId) {
            throw new NotImplementedException();
        }

        public async Task<bool?> DeleteOldApiTokens() {
            _logger.LogInformation("WaterService.DeleteOldApiTokens - Service starts.");

            var oldTokens = await _dbContext.Tokens.Where(t => t.TimeStamp.AddDays(14) < DateTime.UtcNow).Select(t => t).ToListAsync();

            if (oldTokens.Count == 0) return true;
            _dbContext.Tokens.RemoveRange(oldTokens);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("WaterService.DeleteOldApiTokens - Error: " + e);
                return false;
            }

            return true;
        }

        public Task<bool> DeleteUserAlbum(int hidrogenianId, string galleryFolderName = "", string attachmentFolderName = "") {
            throw new NotImplementedException();
        }

        public async Task<AvatarResultVM> SendDeleteAvatarRequestToWater(string apiKey, string photoName) {
            _logger.LogInformation("WaterService.SendDeleteAvatarRequestToWater - Service starts.");

            _waterRequest.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HidroConstants.CONTENT_TYPES["json"]));
            var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(apiKey), "apikey");
            formData.Add(new StringContent(photoName), "image");
            
            var response = await _waterRequest.PostAsync("avatar/remove-avatar", formData);
            if (!response.IsSuccessStatusCode) return null;
            
            AvatarResultVM result;
            try {
                result = JsonConvert.DeserializeObject<AvatarResultVM>(await response.Content.ReadAsStringAsync());
            } catch (Exception e) {
                _logger.LogError("WaterService.SendSaveAvatarRequest - Error: " + e);
                
                var data = await response.Content.ReadAsStringAsync();
                _logger.LogError("JSON Data: " + data);
                return null;
            }
            
            return result;
        }

        public async Task<AvatarResultVM> SendReplaceAvatarRequestToWater(AssetReplaceVM uploading) {
            _logger.LogInformation("WaterService.SendReplaceAvatarRequestToWater - Service starts.");

            _waterRequest.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HidroConstants.CONTENT_TYPES["form"]));
            var formData = new MultipartFormDataContent();

            var formImage = new StreamContent(uploading.File.OpenReadStream());
            formImage.Headers.ContentType = MediaTypeHeaderValue.Parse(uploading.File.ContentType);
            formData.Add(formImage, "replaceBy", uploading.File.FileName);

            formData.Add(new StringContent(uploading.CurrentAvatar), "current");
            formData.Add(new StringContent(uploading.HidrogenianId.ToString()), "hidrogenianId");
            formData.Add(new StringContent(uploading.ApiKey), "apikey");

            var response = await _waterRequest.PostAsync("avatar/replace-avatar", formData);
            if (!response.IsSuccessStatusCode) return null;
            
            AvatarResultVM result;
            try {
                result = JsonConvert.DeserializeObject<AvatarResultVM>(await response.Content.ReadAsStringAsync());
            } catch (Exception e) {
                _logger.LogError("WaterService.SendSaveAvatarRequest - Error: " + e);
                
                var data = await response.Content.ReadAsStringAsync();
                _logger.LogError("JSON Data: " + data);
                return null;
            }
            
            return result;
        }

        public async Task<AvatarResultVM> SendSaveAvatarRequestToWater(AssetFormVM uploading) {
            _logger.LogInformation("WaterService.SendSaveAvatarRequest - Service starts.");

            _waterRequest.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HidroConstants.CONTENT_TYPES["form"]));
            var formData = new MultipartFormDataContent();

            var formImage = new StreamContent(uploading.File.OpenReadStream());
            formImage.Headers.ContentType = MediaTypeHeaderValue.Parse(uploading.File.ContentType);
            formData.Add(formImage, "image", uploading.File.FileName);

            formData.Add(new StringContent(uploading.HidrogenianId.ToString()), "hidrogenianId");
            formData.Add(new StringContent(uploading.ApiKey), "apikey");

            var response = await _waterRequest.PostAsync("avatar/save-avatar", formData);
            if (!response.IsSuccessStatusCode) return null;

            AvatarResultVM result;
            try {
                result = JsonConvert.DeserializeObject<AvatarResultVM>(await response.Content.ReadAsStringAsync());
            } catch (Exception e) {
                _logger.LogError("WaterService.SendSaveAvatarRequest - Error: " + e);
                
                var data = await response.Content.ReadAsStringAsync();
                _logger.LogError("JSON Data: " + data);
                return null;
            }

            return result;
        }

        public async Task<bool> SetApiToken(TokenVM token) {
            _logger.LogInformation("WaterService.SetApiToken - Service starts.");

            var dbToken = new Tokens {
                TokenString = token.Token,
                Life = token.Duration,
                Target = token.Target
            };

            _dbContext.Tokens.Add(dbToken);
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                _logger.LogError("WaterService.SetApiToken - Error: " + e);
                return false;
            }

            return true;
        }
    }
}
