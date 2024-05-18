using Aerospike.Client;
using System.Collections.Concurrent;
using Xunit.Abstractions;
using static Aerospike.Client.AsyncQueryValidate;
using static Aerospike.Client.Value;
using Record = Aerospike.Client.Record;


namespace AeroSpikeXUnit;

public class QueryAsyncTest
{

    [Fact]
    public async Task TestQueryAsync()
    {
        var clientPolicy = new AsyncClientPolicy();
        clientPolicy.timeout = 1000; // milliseconds

        using var client = new AsyncClient(clientPolicy, "127.0.0.1", 3000);


        QueryPolicy queryPolicy = new QueryPolicy() { 
            maxConcurrentNodes = 10,
             compress = true
              
        };
        Statement statement = new Statement() {
            
        Filter = Filter.Equal("AccountId", "\"RA8670f11e03db4553a40a4094e6d74bb4\""),
         SetName = "Campaigns",
         Namespace= "test",
         BinNames = new string[] { "Id", "Name", "AccountId"}
          //Operations

        };

        var records = await client.QueryAsync(queryPolicy, statement);

    }
}


public static class QueryAsyncUtil
{
    public static Task<ConcurrentBag<(Key Key, Record Record)>> QueryAsync(this AsyncClient client, QueryPolicy queryPolicy, Statement statement, object? state = null)
    {        
        MyListener listener = new MyListener(state);       
        client.Query(queryPolicy, listener, statement);
        return listener.Task;
    }

    private class MyListener : RecordSequenceListener
    {
        private readonly TaskCompletionSource<ConcurrentBag<(Key Key, Record Record)>> _ts;

        public MyListener(object? state=null)
        {
            _ts = new TaskCompletionSource<ConcurrentBag<(Key Key, Record Record)>>
            (state, TaskCreationOptions.RunContinuationsAsynchronously);
        }
        internal Task<ConcurrentBag<(Key Key, Record Record)>> Task => _ts.Task;

        private ConcurrentBag<(Key Key, Record Record)> records = new ConcurrentBag<(Key, Record)>();
        public void OnFailure(AerospikeException exception)
        {
            _ts.SetException(exception);
        }

        public void OnRecord(Key key, Aerospike.Client.Record record)
        {
            records.Add((key, record));            
        }

        public void OnSuccess()
        {
            _ts.SetResult(records);
        }
    }

}



