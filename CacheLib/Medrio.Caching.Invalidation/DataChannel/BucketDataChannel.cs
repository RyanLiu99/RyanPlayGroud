using System;
using System.Collections.Generic;
using System.Threading;
using Medrio.Infrastructure.Ioc.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace Medrio.Caching.InMemoryInvalidationService.DataChannel
{
    [RegisterAs(typeof(IBulkDataChannel<>), Lifetime = ServiceLifetime.Singleton)]
    internal class BucketDataChannel<T> : IBulkDataChannel<T>, IDisposable
    {
        private List<T> _collectionBucket = new List<T>();
        private List<T> _processingBucket = new List<T>();

        private readonly object _lockObj = new object();
        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        public void CollectChanges(params T[] dependencies)
        {
            if (dependencies.Length == 0)
            {
                return;
            }

            lock (_lockObj)
            {
                _collectionBucket.AddRange(dependencies);
            }
            _autoResetEvent.Set();
        }

        public IList<T>? WaitForData(int millisecondsTimeout = 5000)
        {
           if(! _autoResetEvent.WaitOne(millisecondsTimeout))
           {
               return null;
           }

           _processingBucket.Clear();
            lock (_lockObj)
            {
                (_processingBucket, _collectionBucket) = (_collectionBucket, _processingBucket);
            }

            return _processingBucket;
        }
        
        public void Dispose()
        {
            try
            {
                _autoResetEvent.Dispose();
            }
            catch 
            {
                //do nothing
            }
        }
    }
}
