using System;
using System.Threading.Tasks;
using Medrio.Caching.Dependencies;
using Medrio.Infrastructure.Ioc.Dependency;

namespace Medrio.Caching.DataChangeNotification.Notifiers
{
    [RegisterAs(typeof(IRemoteInMemoryCacheNotifier))]
    internal class RemoteInMemoryCacheNotifier : IRemoteInMemoryCacheNotifier
    {
        public Task NotifyDatChange(CachingDependencies dependencies)
        {
            throw new NotImplementedException();
        }
    }
}
