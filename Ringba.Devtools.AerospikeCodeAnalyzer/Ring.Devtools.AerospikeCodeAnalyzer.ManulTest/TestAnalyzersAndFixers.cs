using Aerospike.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ring.Devtools.AerospikeCodeAnalyzer.ManulTest
{
    public class TestEnforcingWritePolicy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", 
            Justification = "This unit test just show how code analyzer work")]
        public static void Test()
        {
            //Bad one, do not fix them, they show Analyzers are working 
            var client = new AerospikeClient("localhost", 3000);
            var asyncClient = new AsyncClient(null, "localhost", 3000);
            var wp = new WritePolicy() { durableDelete = false };

            //Good ones
            var client2 = new AerospikeClient(new ClientPolicy(),"localhost", 3000);
            var asyncClient2 = new AsyncClient(new AsyncClientPolicy(), "localhost", 3000);
            var wp2 = new WritePolicy() { durableDelete = true };
        }
    }

    ////Uncomment this to see Analyzer does not apply to WritePolicy not in Aerospike.Clienrt namespace
    //public class WritePolicy
    //{
    //    public bool durableDelete;
    //}
}
