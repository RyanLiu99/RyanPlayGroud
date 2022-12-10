using System;

namespace Medrio.Caching.Abstraction.Caches
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class CacheAttribute : Attribute
    {
        /// <summary>
        /// Cache type for the cache tier type
        /// <remarks>Usually it is an interface.</remarks>
        /// </summary>
        public Type ProviderType { get; private set; }

        public CacheAttribute(Type providerType)
        {
            ProviderType = providerType;
        }
    }
}