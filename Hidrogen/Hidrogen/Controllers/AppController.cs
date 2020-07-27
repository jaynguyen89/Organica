using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using HelperLibrary;
using HelperLibrary.Common;
using Hidrogen.ViewModels.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Hidrogen.Controllers {
    
    public class AppController : ControllerBase {
        
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _redisCache;
        
        private readonly string PROJECT_FOLDER = Path.GetDirectoryName(Directory.GetCurrentDirectory()) + @"/Hidrogen/";

        public AppController() { }

        public AppController(IDistributedCache redisCache) {
            _redisCache = redisCache;
        }

        public AppController(IMemoryCache memoryCache) {
            _memoryCache = memoryCache;
        }

        public AppController(
            IMemoryCache memoryCache,
            IDistributedCache redisCache
        ) {
            _memoryCache = memoryCache;
            _redisCache = redisCache;
        }
        
        /// <summary>
        /// Insert Redis cache entry with: `entryKey` declared in HidroConstants.CACHE_ENTRY_KEYS, `data` being whatever object,
        /// and `isCommon` == false indicating cache entry is associated with the current user id.
        /// </summary>
        protected async Task InsertRedisCacheEntryAsync(string entryKey, object data, bool isCommon = false) {
            var hidrogenianId = HttpContext.Session.GetInt32(nameof(AuthenticatedUser.UserId));
            
            await _redisCache.SetAsync(
                isCommon ?
                    HidroConstants.CACHE_ENTRY_KEYS[entryKey] :
                    $"{HidroConstants.CACHE_ENTRY_KEYS[entryKey]}_{hidrogenianId}",
                HelperProvider.EncodeDataForCache(data),
                new DistributedCacheEntryOptions {
                    SlidingExpiration = TimeSpan.FromDays(HidroConstants.CACHE_SLIDING_EXPIRATION_TIME),
                    AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(HidroConstants.CACHE_ABSOLUTE_EXPIRATION_TIME)
                });
        }

        /// <summary>
        /// Read an entry from Redis cache with: `entryKey` declared in HidroConstants.CACHE_ENTRY_KEYS, 
        /// and `isCommon` == false indicating cache entry is associated with the current user id.
        /// </summary>
        protected async Task<T> ReadFromRedisCacheAsync<T>(string entryKey, bool isCommon = false) {
            var hidrogenianId = HttpContext.Session.GetInt32(nameof(AuthenticatedUser.UserId));

            var cachedData = await _redisCache.GetAsync(
                isCommon ?
                    HidroConstants.CACHE_ENTRY_KEYS[entryKey] :
                    $"{HidroConstants.CACHE_ENTRY_KEYS[entryKey]}_{hidrogenianId}"
            );

            if (cachedData == null) return default;
            
            var data = HelperProvider.DecodeCachedData<T>(cachedData);
            return data;
        }

        protected async Task<string> ReadRawRedisCacheEntry(string entryKey, bool isCommon = false) {
            var hidrogenianId = HttpContext.Session.GetInt32(nameof(AuthenticatedUser.UserId));
            
            var cachedData = await _redisCache.GetAsync(
                isCommon ?
                    HidroConstants.CACHE_ENTRY_KEYS[entryKey] :
                    $"{HidroConstants.CACHE_ENTRY_KEYS[entryKey]}_{hidrogenianId}"
            );

            return cachedData == null ? null : Encoding.UTF8.GetString(cachedData);
        }

        /// <summary>
        /// Insert Memory cache entry with: `entryKey` declared in HidroConstants.CACHE_ENTRY_KEYS, `data` being whatever object,
        /// and `isCommon` == false indicating cache entry is associated with the current user id.
        /// </summary>
        protected void InsertMemoryCacheEntry(
            string entryKey,
            object data,
            int size = 1,
            CacheItemPriority priority = CacheItemPriority.Normal,
            bool isCommon = false
        ) {
            var hidrogenianId = HttpContext.Session.GetInt32(nameof(AuthenticatedUser.UserId));

            _memoryCache.Set(
                isCommon ?
                    HidroConstants.CACHE_ENTRY_KEYS[entryKey] :
                    $"{HidroConstants.CACHE_ENTRY_KEYS[entryKey]}_{hidrogenianId}",
                data,
                priority == CacheItemPriority.NeverRemove ?
                    new MemoryCacheEntryOptions {
                        Priority = priority,
                        Size = size
                    } :
                    new MemoryCacheEntryOptions {
                        Priority = priority,
                        Size = size,
                        SlidingExpiration = TimeSpan.FromDays(HidroConstants.CACHE_SLIDING_EXPIRATION_TIME),
                        AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(HidroConstants.CACHE_ABSOLUTE_EXPIRATION_TIME)
                    }
                );
        }

        /// <summary>
        /// Read an entry from Memory cache with: `entryKey` declared in HidroConstants.CACHE_ENTRY_KEYS, 
        /// and `isCommon` == false indicating cache entry is associated with the current user id.
        /// </summary>
        protected T ReadFromMemoryCache<T>(string entryKey, bool isCommon = false) {
            var hidrogenianId = HttpContext.Session.GetInt32(nameof(AuthenticatedUser.UserId));

            var data = _memoryCache.Get<T>(
                isCommon ?
                HidroConstants.CACHE_ENTRY_KEYS[entryKey] :
                $"{HidroConstants.CACHE_ENTRY_KEYS[entryKey]}_{hidrogenianId}"
            );

            return data;
        }

        /// <summary>
        /// Remove Redis cache entry by the entryKey specified.
        /// </summary>
        protected async Task RemoveRedisCacheEntryAsync(string entryKey) {
            var hidrogenianId = HttpContext.Session.GetInt32(nameof(AuthenticatedUser.UserId));
            await _redisCache.RemoveAsync($"{entryKey}_{hidrogenianId}");
        }
        
        /// <summary>
        /// Remove cache entry from Memory by the entryKey specified.
        /// </summary>
        protected void RemoveMemoryCacheEntry(string entryKey) {
            var hidrogenianId = HttpContext.Session.GetInt32(nameof(AuthenticatedUser.UserId));
            _memoryCache.Remove($"{entryKey}_{hidrogenianId}");
        }
        
        /// <summary>
        /// Parse a file from HtmlTemplates folder to get its content as string.
        /// </summary>
        protected async Task<string> ParseEmailTemplateFromFileWithName(string fileName) {
            using var reader = System.IO.File.OpenText(PROJECT_FOLDER + @"HtmlTemplates/" + fileName);
            var emailTemplate = await reader.ReadToEndAsync();

            return emailTemplate;
        }
    }
}