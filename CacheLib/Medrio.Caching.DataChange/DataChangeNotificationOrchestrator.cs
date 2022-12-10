using System.Linq;
using System.Threading.Tasks;
using Medrio.Caching.DataChangeNotification.Notifiers;
using Medrio.Caching.Dependencies;
using Medrio.Infrastructure.Ioc.Dependency;

namespace Medrio.Caching.DataChangeNotification
{
    [RegisterAs(typeof(IDataChangeNotificationOrchestrator))]
    internal class DataChangeNotificationOrchestrator: IDataChangeNotificationOrchestrator
    {
        private readonly INotifierFactory _notifierFactory;

        public DataChangeNotificationOrchestrator(INotifierFactory notifierFactory)
        {
            _notifierFactory = notifierFactory;
        }

        public Task NotifyDatChange(CachingDependencies dependencies, InvalidationDestinations destinations)
        {
            var notifiers = _notifierFactory.GetNotifiers(destinations);
            var tasks = notifiers.Select(n => n.NotifyDatChange(dependencies));
            return Task.WhenAll(tasks);
        }
    }
}
