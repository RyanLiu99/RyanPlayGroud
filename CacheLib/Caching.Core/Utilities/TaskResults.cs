using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Medrio.Caching.Abstraction.Utilities
{
    public static class TaskResults
    {
        public static readonly Task<bool> True = Task.FromResult(true);
        public static readonly Task<bool> False = Task.FromResult(false);
    }
}
