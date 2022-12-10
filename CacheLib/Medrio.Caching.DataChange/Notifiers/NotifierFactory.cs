using System;
using Medrio.Infrastructure.Ioc.Dependency;

namespace Medrio.Caching.DataChangeNotification.Notifiers
{
    [RegisterAs(typeof(INotifierFactory))]
    internal class NotifierFactory : INotifierFactory
    {
        public IDataChangeNotifier[] GetNotifiers(InvalidationDestinations destinations)
        {
            throw new NotImplementedException();
        }
    }
}
