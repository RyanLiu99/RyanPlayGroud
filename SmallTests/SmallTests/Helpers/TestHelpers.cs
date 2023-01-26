using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace SmallTests.Helpers
{
    internal class TestHelpers
    {
        public static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(() =>
            GlobalSetup.Container?.GetRequiredService<ILoggerFactory>().CreateLogger("TestHelpers")!
        );
    }
}
