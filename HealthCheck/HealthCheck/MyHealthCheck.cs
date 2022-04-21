using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheck
{
    public class MyHealthCheck : IHealthCheck
    {
        public MyHealthCheck()
        {
               Console.WriteLine("MyHealthCheck constructor is called"); 
        }
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("MyHealthCheck CheckHealthAsync() is called");
            return Task.FromResult(HealthCheckResult.Healthy("MyHealthCheck"));
        }
    }
}
