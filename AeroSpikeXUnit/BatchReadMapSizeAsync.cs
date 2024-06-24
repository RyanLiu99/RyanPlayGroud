using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aerospike.Client;
using Xunit.Abstractions;

namespace AeroSpikeXUnit;

public class BatchReadMapSizeAsync
{
    
    private readonly ITestOutputHelper _output;
    
    public BatchReadMapSizeAsync(ITestOutputHelper output)
    {
        _output = output;
    }


    [Fact]
    public async Task ReadMapSizes()
    {

        var binNamesV1 = new string[] {
            "aff",
            "cam",
            "caff",
            "tar",
            "buy",
            "gro",
            "CONc",
            "CONcc",
            "CONs",
            "PAYc",
            "PAYcc",
            "PAYs",
            "PAYsc"
         };

        // var binNames = binNamesV1.SelectMany(x => new string[] { x, x + "_v2" }).ToArray();
        var binNames = binNamesV1.Union(binNamesV1.Select(x =>  x + "_v2" )).ToArray();

        _output.WriteLine("UsageRepo");
        await ReadMapSize("UsageRepo", binNames);
        _output.WriteLine("--------------------");
        _output.WriteLine("UsageRepo_Global");
        await ReadMapSize("UsageRepo_Global", binNames);
    }


    public async Task ReadMapSize(string setName, string[] binNames)
    {
        try
        {
            int port = 3000;


            //https://search-ringba-log-cluster-public-4qophdt6c2i2j4zgy3oj3urwpi.us-east-1.es.amazonaws.com/_dashboards/app/discover#/?_g=(filters:!(),refreshInterval:(pause:!t,value:0),time:(from:now-7d,to:now))&_a=(columns:!(app,LogId,action,argsJson),filters:!(('$state':(store:appState),meta:(alias:!n,disabled:!f,index:'844786b0-311f-11ec-a876-9727ea0caf7b',key:failMessage,negate:!f,params:(query:'Record%20too%20big'),type:phrase),query:(match_phrase:(failMessage:'Record%20too%20big')))),index:'844786b0-311f-11ec-a876-9727ea0caf7b',interval:auto,query:(language:kuery,query:''),sort:!(!(DateTimeString,asc)))


            string host = "10.0.0.95"; //"127.0.0.1";            
            string namespaceName = "ringba";
            string[] bigKeys = new string[] {
                "RAda1c20ad3e7e426fa87a08b42c92d08a",
                "RA0b9590888e254bc68c5d870896d3f74d",
                "RA6b0608e37e674e90a64ac63754c4e8bd",
                "RA49022668e50f4351a22e09d9508ba617",
                "RAaf74a45d14124eecb6fbbe591191c6e6",
                "RAda1c20ad3e7e426fa87a08b42c92d08a",
                "RA31243b25cd754e2b9ebda9ca562eef11",
                "RAda1c20ad3e7e426fa87a08b42c92d08a",
                "RAcf75c21284a34262b1efc5eb7ec979cd"
            };


            //string host = "127.0.0.1";            
            //string namespaceName = "test";
            //string[] bigKeys = new string[] {
            //    "RAcf75c21284a34262b1efc5eb7ec979cd"
            //};

            var keys = bigKeys.Select(k => new Key(namespaceName, setName, k)).ToArray();
            var operations = binNames.Select(b => MapOperation.Size(b)).ToArray();

            // using (AsyncClient _client = new AsyncClient(host, port))
            // {
            //     //require server 6, it is a wrapper around API that take BatchRecordArrayListenerAdapter
            //     BatchResults results = await _client.Operate(BatchPolicy.ReadDefault(), new BatchWritePolicy(), CancellationToken.None, keys, operations);

            //     //another way
            //     //requires server 6
            //     BatchRecordArrayListenerImp listener = new BatchRecordArrayListenerImp();
            //     _client.Operate(BatchPolicy.ReadDefault(), new BatchWritePolicy(), listener, keys, operations);
            //     var t = listener.Task;
            //     (BatchRecord[], bool) result = await t;

            //}

            using (var client = new AerospikeClient(host, port))
            {
                var records = client.Get(BatchPolicy.ReadDefault(), keys, operations);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bigKeys.Length; i++)
                {
                    var record = records[i];
                    sb.Append("Key ").Append(bigKeys[i]).Append("; ");
                    foreach (var bin in record.bins)
                    {
                        sb.AppendFormat("{0}: {1,4};  ", bin.Key, bin.Value);
                    }

                    var camSize = record.GetLong("cam") + record.GetLong("cam_v2");

                    var camRelated = record.GetLong("caff")
                        + record.GetLong("caff_v2")
                         + record.GetLong("CONcc")
                        + record.GetLong("CONsc")
                        + record.GetLong("PAYcc")
                        + record.GetLong("PAYsc");

                    //consider caff as double size, since it has 2 GUIDs
                    var total = record.bins.Values.Sum(x => (long)x) + camRelated;
                    var campRelatedIncludingCamp = camSize + camRelated * 2;
                    sb.AppendFormat("Total: {0,4}; Campaign Related: {1,4}, {2}", total, campRelatedIncludingCamp, total==0? "" : ((decimal)campRelatedIncludingCamp/(decimal)total).ToString("P1")); 
                    _output.WriteLine(sb.ToString());
                    sb.Clear();
                }
            }

        }
        catch (Exception ex)
        {
            _output.WriteLine(ex.ToString());
            throw;
        }
    }
}
