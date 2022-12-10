using System.Threading.Tasks;
using Medrio.Caching.Dependencies;

namespace Medrio.Caching.DataChangeNotification.Notifiers
{
    public interface IDataChangeNotifier
    {
        Task NotifyDatChange(CachingDependencies dependencies);
    }
}

