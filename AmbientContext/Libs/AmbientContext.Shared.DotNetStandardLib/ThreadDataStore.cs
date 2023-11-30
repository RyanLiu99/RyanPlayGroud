using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AmbientContext.Shared.DotNetStandardLib
{
    public static class ThreadDataStore
    {
        private static readonly AsyncLocal<ConcurrentDictionary<string, object>> AsyncLocalDictionary = new ();
                
        public static void AddObject(string key, object value)
        {
            if (AsyncLocalDictionary.Value == null)
            {
                AsyncLocalDictionary.Value = new();
            }
            AsyncLocalDictionary.Value[key] = value;
        }

        public static object GetObject(string key)
        {
            return AsyncLocalDictionary.Value?[key];
        }

        
        public static T GetObject<T>(string key) where T : class
        {
            return (T)GetObject(key);
        }

        public static bool HasKey(string key) => AsyncLocalDictionary.Value?.ContainsKey(key) ?? false;

        public static bool ClearObject(string key) => AsyncLocalDictionary.Value?.TryRemove(key, out _) ?? false;
    }
}
