using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using Log.Abstraction;

namespace Log.Imp.OldHttp
{
    public class LoggerLegacy : ILogger
    {
        public void Log(string message)
        {
            throw new NotImplementedException();
        }

        public void Log(System.Web.HttpRequest request)
        {

        }
    }
}
