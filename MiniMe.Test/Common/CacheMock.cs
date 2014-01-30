using System;
using System.Collections.Generic;
using System.Web.Caching;
using MiniMe.Common;

namespace MiniMe.Test.Common
{
    class CacheMock : ICache
    {
        private static readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>(); 
        public object Get(string key)
        {
            return _dictionary.ContainsKey(key) ? _dictionary[key] : null;
        }

        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            if(_dictionary.ContainsKey(key))
            {
                _dictionary[key] = value;
            }
            else
            {
                _dictionary.Add(key, value);
            }
        }
    }
}