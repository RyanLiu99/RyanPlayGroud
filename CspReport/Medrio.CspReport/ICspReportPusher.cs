using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medrio.CspReport
{
    public interface ICspReportPusher
    {
        Task Push(IList<string> violations);
    }
}
