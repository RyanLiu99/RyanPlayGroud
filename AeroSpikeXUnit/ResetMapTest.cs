using Aerospike.Client;
using AeroSpikeXUnit.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace AeroSpikeXUnit
{
    public class ResetMapTest
    {
        [Fact]
        public async Task TestRestMapEntries()
        {
            var clientPolicy = new AsyncClientPolicy()
            {
                timeout = 1000,// milliseconds
            };

            using var client = new AsyncClient(clientPolicy, "127.0.0.1", 3000);
            var util = new MapUtils(client);

            var keys = KeyUtils.GetKeys("test", "ConRepo2", "RAtestid:dy:6:5", "RAtestid:hr:5:19");
            var dic = new Dictionary<string, string[]>()
            {
                {"CONc",  new string[]{ "TAtestid" } },
                {"CONs", new string[]{ "TAtestid" } }
            };

            var result = await util.RemoveMapEntriesIfExists<string>(keys, dic);
        }


    }
}
