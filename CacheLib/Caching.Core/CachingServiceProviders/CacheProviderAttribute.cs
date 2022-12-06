using System;

namespace Medrio.Caching.Abstraction.CachingServiceProviders
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class CacheProviderAttribute : Attribute
    {
        /// <summary>
        /// Cache provider type for the cache tier type
        /// <remarks>Usually it is an interface.</remarks>
        /// </summary>
        public Type ProviderType { get; private set; }

        public CacheProviderAttribute(Type providerType)
        {
            ProviderType = providerType;
        }
    }
}