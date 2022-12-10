using System.Threading.Tasks;
using Medrio.Caching.Dependencies;

namespace Medrio.Caching.DataChangeNotification
{
    public interface IDataChangeNotificationOrchestrator
    {
        Task NotifyDatChange(CachingDependencies dependencies, InvalidationDestinations destinations);
    }
}
