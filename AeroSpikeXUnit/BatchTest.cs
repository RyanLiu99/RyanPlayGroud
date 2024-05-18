using Aerospike.Client;
using System;
using Xunit.Abstractions;
using static Aerospike.Client.Value;


namespace AeroSpikeXUnit;

public class BatchTest
{
    private readonly ITestOutputHelper _output;

    private WritePolicy _writePolicy = new WritePolicy
    {
        expiration = -2, //seconds
        sendKey = true

    };

    public BatchTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void TestBatchRead()  // Need server 6
    {
        using var client = new AerospikeClient(null, "127.0.0.1", 3000);

        Parameters parameters = new Parameters()
        {
            NS = "test",
            Set = "batchTest"
        };

        string BinName1 = "Bin1";
        string BinName2 = "Bin2";
        string BinName4 = "Bin4";

        string ResultName1 = "Result1";
        string ResultName2 = "Result2";

        string KeyPrefix = "BatchTest_";


        //Expression wexp1 = Exp.Build(Exp.Add(Exp.IntBin(BinName1), Exp.IntBin(BinName2), Exp.Val(1000)));
        Expression rexp1 = Exp.Build(Exp.Mul(Exp.IntBin(BinName1), Exp.IntBin(BinName2)));
        Expression rexp2 = Exp.Build(Exp.Add(Exp.IntBin(BinName1), Exp.IntBin(BinName2)));
        Expression rexp3 = Exp.Build(Exp.Sub(Exp.IntBin(BinName1), Exp.IntBin(BinName2)));

        Operation[] ops1 = Operation.Array(
            Operation.Put(new Bin(BinName1, 1)),
            Operation.Put(new Bin(BinName2, 2)),
            Operation.Put(new Bin(BinName4, 100)),
            ExpOperation.Read(ResultName1, rexp1, ExpReadFlags.EVAL_NO_FAIL)
            );

        client.Operate(new WritePolicy(), new Key(parameters.NS, parameters.Set, KeyPrefix + "1"), ops1);

        Operation[] ops2 = Operation.Array(ExpOperation.Read(ResultName1, rexp1, ExpReadFlags.EVAL_NO_FAIL));
        Operation[] ops3 = Operation.Array(ExpOperation.Read(ResultName1, rexp2, ExpReadFlags.EVAL_NO_FAIL));

        Operation[] ops4 = Operation.Array(
            //ExpOperation.Write(BinName1, wexp1, ExpWriteFlags.EVAL_NO_FAIL),
            ExpOperation.Read(ResultName1, rexp3, ExpReadFlags.EVAL_NO_FAIL));

        Operation[] ops5 = Operation.Array(
            ExpOperation.Read(ResultName1, rexp2, ExpReadFlags.EVAL_NO_FAIL),
            ExpOperation.Read(ResultName2, rexp3, ExpReadFlags.EVAL_NO_FAIL));

        List<BatchRecord> batchRecords = new List<BatchRecord>() {
       // new BatchWrite(new Key(parameters.NS, parameters.Set, KeyPrefix + "1"), ops1), //require upgrade server to 6
        new BatchRead(new Key(parameters.NS, parameters.Set, KeyPrefix + "2"), ops2),
        new BatchRead(new Key(parameters.NS, parameters.Set, KeyPrefix + "3"), ops3),
      //  new BatchWrite(new Key(parameters.NS, parameters.Set, KeyPrefix + "4"), ops4),
        new BatchRead(new Key(parameters.NS, parameters.Set, KeyPrefix + "5"), ops5),
        new BatchDelete(new Key(parameters.NS, parameters.Set, KeyPrefix + "6"))
        };

        


        // Execute batch.
        client.Operate(new BatchPolicy(), batchRecords);

        // Show results.
        int i = 0;
        foreach (BatchRecord record in batchRecords)
        {
            var rec = record.record;

            if (rec != null)
            {
                Object v1 = rec.GetValue(ResultName1);
                Object v2 = rec.GetValue(ResultName2);

            }
            else
            {
                _output.WriteLine("Result[%d]: error: %s", i, ResultCode.GetResultString(record.resultCode));
            }
            i++;
        }
    }


    // [Fact]
    public void TestBatchReadWriter_RequiareServer6()
    {
        Parameters parameters = new Parameters()
        {
            NS = "test",
            Set = "batchTest"
        };

        string BinName1 = "Bin1";
        string BinName2 = "Bin2";     
        string BinName4 = "Bin4";

        string ResultName1 = "Result1";
        string ResultName2 = "Result2";

        string KeyPrefix = "BatchTest_";


        Expression wexp1 = Exp.Build(Exp.Add(Exp.IntBin(BinName1), Exp.IntBin(BinName2), Exp.Val(1000)));
        Expression rexp1 = Exp.Build(Exp.Mul(Exp.IntBin(BinName1), Exp.IntBin(BinName2)));
        Expression rexp2 = Exp.Build(Exp.Add(Exp.IntBin(BinName1), Exp.IntBin(BinName2)));
        Expression rexp3 = Exp.Build(Exp.Sub(Exp.IntBin(BinName1), Exp.IntBin(BinName2)));

        Operation[] ops1 = Operation.Array(
            Operation.Put(new Bin(BinName1, 1)),
            Operation.Put(new Bin(BinName2, 2)),
            Operation.Put(new Bin(BinName4, 100)),
            ExpOperation.Read(ResultName1, rexp1, ExpReadFlags.DEFAULT)
            );

        Operation[] ops2 = Operation.Array(ExpOperation.Read(ResultName1, rexp1, ExpReadFlags.DEFAULT));
        Operation[] ops3 = Operation.Array(ExpOperation.Read(ResultName1, rexp2, ExpReadFlags.DEFAULT));

        Operation[] ops4 = Operation.Array(
            ExpOperation.Write(BinName1, wexp1, ExpWriteFlags.DEFAULT),
            ExpOperation.Read(ResultName1, rexp3, ExpReadFlags.DEFAULT));

        Operation[] ops5 = Operation.Array(
            ExpOperation.Read(ResultName1, rexp2, ExpReadFlags.DEFAULT),
            ExpOperation.Read(ResultName2, rexp3, ExpReadFlags.DEFAULT));

        List<BatchRecord> batchRecords = new List<BatchRecord>() {
        new BatchWrite(new Key(parameters.NS, parameters.Set, KeyPrefix + "1"), ops1), //require upgrade server to 6
        new BatchRead(new Key(parameters.NS, parameters.Set, KeyPrefix + "2"), ops2),
        new BatchRead(new Key(parameters.NS, parameters.Set, KeyPrefix + "3"), ops3),
        new BatchWrite(new Key(parameters.NS, parameters.Set, KeyPrefix + "4"), ops4),
        new BatchRead(new Key(parameters.NS, parameters.Set, KeyPrefix + "5"), ops5),
        new BatchDelete(new Key(parameters.NS, parameters.Set, KeyPrefix + "6"))
        };

        using var client = new AerospikeClient(null, "127.0.0.1", 3000);

      

        // Execute batch.
        client.Operate(new BatchPolicy(), batchRecords);

        // Show results.
        int i = 0;
        foreach (BatchRecord record in batchRecords)
        {
            var rec = record.record;

            if (rec != null)
            {
                Object v1 = rec.GetValue(ResultName1);
                Object v2 = rec.GetValue(ResultName2);

            }
            else
            {
                _output.WriteLine("Result[%d]: error: %s", i, ResultCode.GetResultString(record.resultCode));
            }
            i++;
        }
    }



    public class Parameters
    {
        public string Set { get; internal set; }
        public string NS { get; internal set; }
    }
}

