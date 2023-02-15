using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmallTests.Helpers
{
    public static class WaitHelper
    {
        /// <summary>
        /// Keep checking and wait for <paramref name="check"/> to return true 
        /// </summary>
        /// <param name="check">Func to invoke to check for condition. Returning true will terminate wait and return </param>
        /// <param name="intervalMs">how long to wait in each iteration.</param>
        /// <param name="iterations">Max times to run.</param>
        /// <returns>result of last call to <paramref name="check"/>.</returns>
        public static bool SpinWait(Func<bool> check, int intervalMs=5, int iterations=200)
        {
            for (int i = 0; i < iterations; i++)
            {
                if (check()) return true;
                Thread.Sleep(intervalMs);
            }

            return check();
        }

        /// <summary>
        /// Keep checking and wait for <paramref name="check"/> to return true 
        /// </summary>
        /// <param name="check">Func to invoke to check for condition. Returning true will terminate wait and return </param>
        /// <param name="intervalMs">how long to wait in each iteration.</param>
        /// <param name="iterations">Max times to run.</param>
        /// <returns>result of last call to <paramref name="check"/>.</returns>
        public static async Task<bool> SpinWaitAsync(Func<bool> check, int intervalMs=5, int iterations=200)
        {
            for (int i = 0; i < iterations; i++)
            {
                if (check()) return true;
                await Task.Delay(intervalMs).ConfigureAwait(false);
            }

            return check();
        }
    }
}
