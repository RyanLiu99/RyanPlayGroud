using System;

namespace Medrio.Caching.DataChangeNotification.Notifiers
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class NotifierAttribute : Attribute
    {
        /// <summary>
        /// Cache provider type for the cache tier type
        /// <remarks>Usually it is an interface.</remarks>
        /// </summary>
        public Type ProviderType { get; private set; }

        public NotifierAttribute(Type providerType)
        {
            ProviderType = providerType;
        }
    }
}