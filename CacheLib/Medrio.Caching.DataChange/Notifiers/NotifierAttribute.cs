using System;

namespace Medrio.Caching.DataChangeNotification.Notifiers
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class NotifierAttribute : Attribute
    {
        /// <summary>
        /// Notifier type 
        /// <remarks>Usually it is an interface.</remarks>
        /// </summary>
        public Type NotifierType { get; private set; }

        public NotifierAttribute(Type notifierType)
        {
            NotifierType = notifierType;
        }
    }
}