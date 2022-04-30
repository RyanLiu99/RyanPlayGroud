using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheck
{
    public class TypeActivatedCheck : IHealthCheck, IDisposable
    {
        public IArgDependency Arg { get; }
        public string Title { get; }

        /*
         * If pass in, it is used, won't resolve from Ioc container.
         * If not pass in (missing), it will resolve from ioc container.  Ioc container life time applies.
         * Arguments order in constructor does not matter.  Injected and passed in arges, either can be first.  Ocz caller should respect constructor.
         */
        public TypeActivatedCheck(string title, IArgDependency arg)
        {
            Console.WriteLine($"TypeActivatedCheck constructor called with Arg {arg.Id}, Title {title}");
            Arg = arg;
            Title = title;
        }


        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("TypeActivatedCheck CheckHealthAsync() called");
            return Task.FromResult(HealthCheckResult.Unhealthy("TypeActivatedCheck"));
        }

        public void Dispose()
        {
            Console.WriteLine("TypeActivatedCheck Dispose() is called");
        }
    }
}
