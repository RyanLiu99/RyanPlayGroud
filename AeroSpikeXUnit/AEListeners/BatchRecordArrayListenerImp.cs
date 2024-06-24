using Aerospike.Client;
using System;
using System.Collections.Concurrent;
using Record = Aerospike.Client.Record;


namespace AeroSpikeXUnit.AEListeners;

public class BatchRecordArrayListenerImp : BatchRecordArrayListener
{
    private readonly TaskCompletionSource<(BatchRecord[], bool)> _ts;

    public Task<(BatchRecord[], bool)> Task => _ts.Task;

    public BatchRecordArrayListenerImp(object? state = null)
    {
        _ts = new TaskCompletionSource<(BatchRecord[], bool)>
        (state, TaskCreationOptions.RunContinuationsAsynchronously);
    }

    public void OnFailure(BatchRecord[] records, AerospikeException ae)
    {
        _ts.SetException(ae);
    }

    public void OnSuccess(BatchRecord[] records, bool status)
    {
        _ts.SetResult((records, status));
    }
}



