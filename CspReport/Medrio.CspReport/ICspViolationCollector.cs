using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medrio.CspReport
{
    public interface ICspViolationCollector
    {
        Task Collect(Stream violation);
        IList<string>? BlockToHandOverData(int millisecondsTimeout = 5000);

        IList<string> WindDown();
    }
}
