using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Caching.Interfaces;
using System.Runtime.Caching;

namespace Thesis.Caching.Providers
{
    public class DefaultCacheProvider : ICacheProvider
    {
        private ObjectCache Cache { get { return MemoryCache.Default; } }

        public virtual object Get(string key)
        {
            return Cache[key];
        }

        public virtual void Set(string key, object data, int cacheTime)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);

            Cache.Add(new CacheItem(key, data), policy);
        }

        public bool IsSet(string key)
        {
            return Cache.Contains(key);
        }

        public void Remove(string key)
        {
            if(Cache.Contains(key))
                Cache.Remove(key);
        }
    }
}
