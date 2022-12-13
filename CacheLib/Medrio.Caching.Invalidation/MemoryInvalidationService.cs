using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Medrio.Caching.Dependencies;
using Medrio.Caching.InMemoryInvalidation.DataChannel;

namespace Medrio.Caching.InMemoryInvalidation
{
    internal class MemoryCacheInvalidationService 
    {
        private readonly IDependenciesDataChannel _channel;

        public MemoryCacheInvalidationService(IDependenciesDataChannel channel)
        {
            _channel = channel;
        }

        public void DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                IList<CachingDependencies>? changes = _channel.WaitForData();

                if (changes != null && changes.Any())
                {
                    CachingDependencies? c = changes.Compress();
                    if (c != null)
                    {
                        ProcessChange(c);
                    }
                }
            }
        }

        private void ProcessChange(CachingDependencies cachingDependencies)
        {
            throw new NotImplementedException();
        }
    }
}
