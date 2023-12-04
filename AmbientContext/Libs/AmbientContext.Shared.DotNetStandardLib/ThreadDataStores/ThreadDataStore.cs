using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AmbientContext.Shared.DotNetStandardLib.ThreadDataStores
{
    public static class ThreadDataStore
    {
        // works with .NET core/.NET, but not all cases in .NET framework
        // private static readonly IImpThreadDataStore ImpInstance = new AsyncLocalThreadDataStore(); 


        // works in .NET core/.NET, and .NET framework, works everywhere
        private static readonly IImpThreadDataStore ImpInstance = new PrincipalThreadDataStore();

        public static void AddObject(string key, object value)
        {
            ImpInstance.AddObject(key, value);
        }

        public static object GetObject(string key)
        {
          return ImpInstance.GetObject(key);
        }

        public static T GetObject<T>(string key) where T : class
        {
            return (T)GetObject(key);
        }


        public static bool HasKey(string key)
        {
            return ImpInstance.HasKey(key);
        }

        public static bool ClearObject(string key) => ImpInstance.ClearObject(key);
    }
}
