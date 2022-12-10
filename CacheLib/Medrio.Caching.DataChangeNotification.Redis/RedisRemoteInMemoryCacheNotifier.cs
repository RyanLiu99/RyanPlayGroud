using System;
using System.Threading.Tasks;
using Medrio.Caching.DataChangeNotification.Notifiers;
using Medrio.Caching.Dependencies;
using Medrio.Infrastructure.Ioc.Dependency;

namespace Medrio.Caching.DataChangeNotification.Redis
{
    [RegisterAs(typeof(IRemoteInMemoryCacheNotifier))]
    internal class RedisRemoteInMemoryCacheNotifier : IRemoteInMemoryCacheNotifier
    {
        public Task NotifyDatChange(CachingDependencies dependencies)
        {
            throw new NotImplementedException();
        }
    }
}
