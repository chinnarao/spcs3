using System;
using Microsoft.Extensions.Caching.Memory;

namespace Services.Commmon
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public T GetOrAdd<T>(string cacheKey, Func<T> factory, DateTime absoluteExpiration)
        {
            // locks get and set internally but call to factory method is not locked
            return _memoryCache.GetOrCreate(cacheKey, entry => factory());
        }

        public T Get<T>(string cacheKey)
        {
            return _memoryCache.Get<T>(cacheKey);
        }
    }

    public class LockedFactoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public LockedFactoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public T GetOrAdd<T>(string cacheKey, Func<T> factory, DateTime absoluteExpiration)
        {
            // locks get and set internally
            if (_memoryCache.TryGetValue<T>(cacheKey, out var result))
            {
                return result;
            }

            lock (TypeLock<T>.Lock)
            {
                if (_memoryCache.TryGetValue(cacheKey, out result))
                {
                    return result;
                }

                result = factory();
                _memoryCache.Set(cacheKey, result, absoluteExpiration);

                return result;
            }
        }

        public T GetOrAdd<T>(string cacheKey, Func<T> factory, DateTimeOffset absoluteExpiration)
        {
            // locks get and set internally
            if (_memoryCache.TryGetValue<T>(cacheKey, out var result))
            {
                return result;
            }

            lock (TypeLock<T>.Lock)
            {
                if (_memoryCache.TryGetValue(cacheKey, out result))
                {
                    return result;
                }

                result = factory();
                _memoryCache.Set(cacheKey, result, absoluteExpiration);

                return result;
            }
        }

        public T Get<T>(string cacheKey)
        {
            if (_memoryCache.TryGetValue<T>(cacheKey, out var result))
            {
                return result;
            }

            lock (TypeLock<T>.Lock)
            {
                if (_memoryCache.TryGetValue(cacheKey, out result))
                {
                    return result;
                }
                return result;
            }
        }

        private static class TypeLock<T>
        {
            public static object Lock { get; } = new object();
        }
    }

    public interface ICacheService
    {
        T GetOrAdd<T>(string cacheKey, Func<T> factory, DateTime absoluteExpiration);
        T Get<T>(string cacheKey);
    }
}
