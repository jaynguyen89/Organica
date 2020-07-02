using System;
using System.Threading.Tasks;
using HelperLibrary;
using HelperLibrary.Common;
using Hidrogen.ViewModels.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Hidrogen.Services {
    
    public class HidroServiceBase {
        
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _redisCache;
        private readonly HttpContext _httpContext;

        protected HidroServiceBase() { }

        protected HidroServiceBase(
            IDistributedCache redisCache,
            IHttpContextAccessor httpContextAccessor
        ) {
            _redisCache = redisCache;
            _httpContext = httpContextAccessor.HttpContext;
        }

        protected HidroServiceBase(
            IMemoryCache memoryCache,
            IHttpContextAccessor httpContextAccessor
        ) {
            _memoryCache = memoryCache;
            _httpContext = httpContextAccessor.HttpContext;
        }

        protected HidroServiceBase(
            IDistributedCache redisCache,
            IMemoryCache memoryCache,
            IHttpContextAccessor httpContextAccessor
        ) {
            _redisCache = redisCache;
            _memoryCache = memoryCache;
            _httpContext = httpContextAccessor.HttpContext;
        }
        
        /// <summary>
        /// Insert Redis cache entry with: `entryKey` declared in HidroConstants.CACHE_ENTRY_KEYS, `data` being whatever object,
        /// and `isCommon` == false indicating cache entry is associated with the current user id.
        /// </summary>
        protected async Task InsertRedisCacheEntryAsync(string entryKey, object data, bool isCommon = false) {
            var hidrogenianId = _httpContext.Session.GetInt32(nameof(AuthenticatedUser.UserId));
            
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
            var hidrogenianId = _httpContext.Session.GetInt32(nameof(AuthenticatedUser.UserId));

            var cachedData = await _redisCache.GetAsync(
                isCommon ?
                    HidroConstants.CACHE_ENTRY_KEYS[entryKey] :
                    $"{HidroConstants.CACHE_ENTRY_KEYS[entryKey]}_{hidrogenianId}"
            );

            if (cachedData == null) return default;
            
            var data = HelperProvider.DecodeCachedData<T>(cachedData);
            return data;
        }

        /// <summary>
        /// Insert Memory cache entry with: `entryKey` declared in HidroConstants.CACHE_ENTRY_KEYS, `data` being whatever object,
        /// and `isCommon` == false indicating cache entry is associated with the current user id.
        /// </summary>
        protected void InsertMemoryCacheEntry(
            string entryKey,
            object data,
            int size = 1,
            string keySuffix = null,
            CacheItemPriority priority = CacheItemPriority.Normal,
            bool isCommon = false
        ) {
            var suffix = string.IsNullOrEmpty(keySuffix)
                                ? _httpContext.Session.GetInt32(nameof(AuthenticatedUser.UserId)).ToString()
                                : keySuffix;

            _memoryCache.Set(
                isCommon ?
                    HidroConstants.CACHE_ENTRY_KEYS[entryKey] :
                    $"{HidroConstants.CACHE_ENTRY_KEYS[entryKey]}_{suffix}",
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
        protected T ReadFromMemoryCache<T>(string entryKey, string keySuffix = null, bool isCommon = false) {
            var suffix = string.IsNullOrEmpty(keySuffix)
                                ? _httpContext.Session.GetInt32(nameof(AuthenticatedUser.UserId)).ToString()
                                : keySuffix;

            var data = _memoryCache.Get<T>(
                isCommon ?
                HidroConstants.CACHE_ENTRY_KEYS[entryKey] :
                $"{HidroConstants.CACHE_ENTRY_KEYS[entryKey]}_{suffix}"
            );

            return data;
        }

        /// <summary>
        /// Remove Redis cache entry by the entryKey specified.
        /// </summary>
        protected async Task RemoveRedisCacheEntryAsync(string entryKey) {
            var hidrogenianId = _httpContext.Session.GetInt32(nameof(AuthenticatedUser.UserId));
            await _redisCache.RemoveAsync($"{entryKey}_{hidrogenianId}");
        }
        
        /// <summary>
        /// Remove cache entry from Memory by the entryKey specified.
        /// </summary>
        protected void RemoveMemoryCacheEntry(string entryKey, string keySuffix = null) {
            var suffix = string.IsNullOrEmpty(keySuffix)
                                ? _httpContext.Session.GetInt32(nameof(AuthenticatedUser.UserId)).ToString()
                                : keySuffix;
            
            _memoryCache.Remove($"{entryKey}_{suffix}");
        }
    }
}