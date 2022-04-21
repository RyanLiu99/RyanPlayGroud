﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheck
{
    public class TypeActivatedCheck : IHealthCheck, IDisposable
    {
        public TypeActivatedCheck()
        {
            Console.WriteLine("TypeActivatedCheck constructor called");
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
