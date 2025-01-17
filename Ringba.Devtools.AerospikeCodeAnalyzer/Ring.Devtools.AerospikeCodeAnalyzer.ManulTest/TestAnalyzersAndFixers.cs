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
        public static WritePolicy Test()
        {
            return new WritePolicy() { durableDelete = false };
        }
    }


    //public class WritePolicy
    //{
    //    public bool durableDelete;
    //}
}
