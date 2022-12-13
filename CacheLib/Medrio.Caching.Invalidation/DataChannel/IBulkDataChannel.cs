using System.Collections.Generic;

namespace Medrio.Caching.InMemoryInvalidation.DataChannel
{
    public interface IBulkDataChannel<T>
    {
        void CollectChanges(params T[] dependencies);
        IList<T>? WaitForData(int millisecondsTimeout = 5000);
    }
}
