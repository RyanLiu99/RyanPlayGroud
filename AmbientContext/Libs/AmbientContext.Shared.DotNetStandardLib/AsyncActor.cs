using AmbientContext.Shared.DotNetStandardLib.Models;
using System.Threading;
using System.Threading.Tasks;

namespace AmbientContext.Shared.DotNetStandardLib
{
    public class AsyncActor
    {
        public static async Task DoSthAsync()
        {
            await Task.Delay(10);
            await Task.Delay(10).ConfigureAwait(false);
        }
    }
}
