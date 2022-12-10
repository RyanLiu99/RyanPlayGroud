using System;
using System.Threading.Tasks;
using Medrio.Caching.Dependencies;
using Medrio.Infrastructure.Ioc.Dependency;

namespace Medrio.Caching.DataChangeNotification.Notifiers
{
    /// <inheritdoc />
    [RegisterAs(typeof(ILocalInMemoryCacheNotifier))]
    internal class LocalInMemoryCacheNotifier : ILocalInMemoryCacheNotifier
    {
        public Task NotifyDatChange(CachingDependencies dependencies)
        {
            throw new NotImplementedException();
        }
    }
}
