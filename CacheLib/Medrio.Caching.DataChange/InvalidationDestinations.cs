using System;
using Medrio.Caching.DataChangeNotification.Notifiers;

namespace Medrio.Caching.DataChangeNotification
{
    [Flags]
    public enum InvalidationDestinations : byte
    {
        [Notifier(typeof(ILocalInMemoryCacheNotifier))]
        LocalInMemory,

        [Notifier(typeof(IRemoteInMemoryCacheNotifier))]
        RemoteInMemory,

        [Notifier(typeof(IDistributedCacheNotifier))]
        Distributed
    }
}
