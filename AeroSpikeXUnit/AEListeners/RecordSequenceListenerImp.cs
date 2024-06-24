using Aerospike.Client;
using System.Collections.Concurrent;
using Record = Aerospike.Client.Record;


namespace AeroSpikeXUnit.AEListeners;

public class RecordSequenceListenerImp : RecordSequenceListener
{
    private readonly TaskCompletionSource<ConcurrentBag<(Key Key, Record Record)>> _ts;

    public RecordSequenceListenerImp(object? state = null)
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

    public void OnRecord(Key key, Record record)
    {
        records.Add((key, record));
    }

    public void OnSuccess()
    {
        _ts.SetResult(records);
    }
}




