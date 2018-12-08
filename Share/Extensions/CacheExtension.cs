using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

//http://hishambinateya.com/cache-dependency-in-asp.net-core
//https://tpodolak.com/blog/2017/12/13/asp-net-core-memorycache-getorcreate-calls-factory-method-multiple-times/
//https://github.com/tpodolak/Blog/tree/master/AspNetCoreMemoryCacheIsPopulatedMultipleTimes
//https://www.c-sharpcorner.com/article/asp-net-core-in-memory-caching/
//https://github.com/ttrider/CacheAdapterAsync
namespace Microsoft.Extensions.Caching
{
    public static class Extensions
    {
        static readonly ConcurrentDictionary<string, object> MemoryCacheRequestDictionary = new ConcurrentDictionary<string, object>();
        static readonly ConcurrentDictionary<string, object> DistributedCacheRequestDictionary = new ConcurrentDictionary<string, object>();

        public static async Task<T> GetOrCreateExclusiveAsync<T>(this IMemoryCache cache, string key, Func<string, Task<T>> factoryMethod)
        {
            if (cache == null) throw new ArgumentNullException(nameof(cache));
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (factoryMethod == null) throw new ArgumentNullException(nameof(factoryMethod));

            object result;
            if (!cache.TryGetValue(key, out result))
            {
                var asyncLazy = (AsyncLazy<T>)MemoryCacheRequestDictionary.GetOrAdd(key, (k) =>
                {

                    var newAsyncLazy = new AsyncLazy<T>(k, async (kk) =>
                    {
                        var value = await factoryMethod(kk);
                        cache.Set(k, value);
                        object oldAsync;
                        MemoryCacheRequestDictionary.TryRemove(k, out oldAsync);
                        return value;
                    });

                    return newAsyncLazy;
                });

                return await asyncLazy.Value;
            }

            return (T)result;
        }



        public static async Task<byte[]> GetOrCreateExclusiveAsync(this IDistributedCache cache, string key, Func<string, Task<byte[]>> factoryMethod)
        {
            if (cache == null) throw new ArgumentNullException(nameof(cache));
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (factoryMethod == null) throw new ArgumentNullException(nameof(factoryMethod));

            byte[] result = await cache.GetAsync(key);
            if (result == null)
            {
                var asyncLazy = (AsyncLazy<byte[]>)DistributedCacheRequestDictionary.GetOrAdd(key, (k) =>
                {

                    var newAsyncLazy = new AsyncLazy<byte[]>(k, async (kk) =>
                    {
                        var value = await factoryMethod(kk);
                        await cache.SetAsync(k, value);
                        object oldAsync;
                        DistributedCacheRequestDictionary.TryRemove(k, out oldAsync);
                        return value;
                    });
                    return newAsyncLazy;
                });
                return await asyncLazy.Value;
            }
            return result;
        }
    }

    public class AsyncLazy<T> : Lazy<Task<T>>
    {
        public AsyncLazy(Func<T> valueFactory) :
            base(() => Task.Factory.StartNew(valueFactory))
        { }

        public AsyncLazy(string key, Func<string, Task<T>> taskFactory) :
            base(() => Task.Factory.StartNew(() => taskFactory(key)).Unwrap())
        { }

        public TaskAwaiter<T> GetAwaiter() { return Value.GetAwaiter(); }
    }
}
