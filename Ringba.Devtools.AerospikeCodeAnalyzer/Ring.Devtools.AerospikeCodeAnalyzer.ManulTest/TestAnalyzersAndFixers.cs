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
        public static void Test()
        {
            var client = new AerospikeClient("localhost", 3000);

            var wp = new WritePolicy() { durableDelete = false };
        }
    }


    //public class WritePolicy
    //{
    //    public bool durableDelete;
    //}
}
