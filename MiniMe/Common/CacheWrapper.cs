using System;
using System.Web;
using System.Web.Caching;

namespace MiniMe.Common
{
    class CacheWrapper : ICache
    {
        public object Get(string key)
        {
            return HttpContext.Current.Cache.Get(key);
        }

        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            HttpContext.Current.Cache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration);
        }
    }
}