using Aerospike.Client;

namespace Ring.Devtools.Aerospike.ManulTest
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
