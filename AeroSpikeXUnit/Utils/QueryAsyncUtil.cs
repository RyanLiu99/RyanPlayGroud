using Aerospike.Client;
using AeroSpikeXUnit.AEListeners;
using System.Collections.Concurrent;
using Record = Aerospike.Client.Record;


namespace AeroSpikeXUnit.Utils;

public static partial class QueryAsyncUtil
{
    public static Task<ConcurrentBag<(Key Key, Record Record)>> QueryAsync(this AsyncClient client, QueryPolicy queryPolicy, Statement statement, object? state = null)
    {
        RecordSequenceListenerImp listener = new RecordSequenceListenerImp(state);
        client.Query(queryPolicy, listener, statement);
        return listener.Task;
    }

}



