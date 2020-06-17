using System;
using Microsoft.Extensions.Caching.Memory;

namespace Hidrogen.Services.ApplicationServices {
    
    public class HidroMemoryCache {
        
        public MemoryCache MemoryCache { get; set; }

        public HidroMemoryCache() {
            MemoryCache = new MemoryCache(new MemoryCacheOptions {
                SizeLimit = 1989, //The number of items that can be cached in memory
                CompactionPercentage = 0.5, //Remove 50% cached items on full cache
                ExpirationScanFrequency = TimeSpan.FromSeconds(3600) //1 hour
            });
        }
    }
}