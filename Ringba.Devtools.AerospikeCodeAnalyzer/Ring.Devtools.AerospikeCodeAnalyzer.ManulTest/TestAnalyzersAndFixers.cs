using Aerospike.Client;

namespace Ring.Devtools.AerospikeCodeAnalyzer.ManulTest
{
    public class TestEnforcingWritePolicy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", 
            Justification = "This unit test just show how code analyzer work")]
        public static void Test()
        {
            //Bad one, do not fix them, they show Analyzers are working 
            var clientPolicy1 = new AsyncClientPolicy(); // Wrong: no writePolicyDefault

            var client11 = new AerospikeClient("localhost", 3000); // Wrong: No ClientPolicy
            var client12 = new AerospikeClient( null, "localhost", 3000); // Wrong: CleintPolicy is null

            var asyncClient11 = new AsyncClient("localhost", 3000);
            var asyncClient12 = new AsyncClient(null, "localhost", 3000);

            var wp11 = new WritePolicy() { durableDelete = false };
            var wp12 = new WritePolicy();


            //Good ones
            var clientPolicy2 = new AsyncClientPolicy() {writePolicyDefault= new WritePolicy() { durableDelete = true} };

            var client2 = new AerospikeClient(clientPolicy2, "localhost", 3000);            
            var asyncClient2 = new AsyncClient(clientPolicy2, "localhost", 3000);
            var wp2 = new WritePolicy() { durableDelete = true };
        }
    }

    ////Uncomment this to see Analyzer does not apply to WritePolicy not in Aerospike.Clienrt namespace
    //public class WritePolicy
    //{
    //    public bool durableDelete;
    //}
}
