using System;
using System.Threading.Tasks;
using Medrio.Caching.DataChangeNotification.Notifiers;
using Medrio.Caching.Dependencies;
using Medrio.Infrastructure.Ioc.Dependency;

namespace Medrio.Caching.DataChangeNotification.Redis
{
    [RegisterAs(typeof(IDistributedCacheNotifier))]
    internal class RedisDistributedCacheNotifier : IDistributedCacheNotifier
    {
        public Task NotifyDatChange(CachingDependencies dependencies)
        {
            throw new NotImplementedException();
        }
    }
}
