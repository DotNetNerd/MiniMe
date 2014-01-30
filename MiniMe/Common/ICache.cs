using System;
using System.Web.Caching;

namespace MiniMe.Common
{
    public interface ICache
    {
        object Get(string key);
        void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration);
    }
}