using Aerospike.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Aerospike.Client.Value;

namespace AeroSpikeXUnit
{
    public class ExpOperationTest
    {
        [Fact]
        public void TestExpOperation()
        {
            // Initialize the Aerospike client
            AerospikeClient client = new AerospikeClient("127.0.0.1", 3000);
            Key key = new Key("ryantest", "expOp", "key1");

            client.Delete(null, key);
            // Write initial values to bin1 and bin2
            Bin bin1 = new Bin("bin1", 10);
            Bin bin2 = new Bin("bin2", 20);
            client.Put(null, key, bin1, bin2);


            // Define an expression to add the values of bin1 and bin2
            Exp addExp = Exp.Add(Exp.IntBin("bin1"), Exp.IntBin("bin2"));

            // Wrap the Exp object in an Expression object
            Expression expression = Exp.Build(addExp);

            // Use ExpOperation to evaluate the expression and write the result to 'resultBin'
            Operation[] operations = new Operation[]
            {
                    ExpOperation.Write("resultBin", expression, ExpWriteFlags.DEFAULT),
                    Operation.Get("resultBin") // Optional: get the result immediately after writing
            };

            // Apply the operations on the specified key
            var record = client.Operate(null, key, operations);

            var r = record.bins["resultBin"] as List<object>;//  Aerospike.Client.RecordParser.OpResults: List<object>// is a private inner class
            Assert.Equal(30, (long)r[1]);

            // Close the client connection
            client.Close();
        }

        [Fact]
        public void TestNestedMap()
        {
            using AerospikeClient client = new AerospikeClient("127.0.0.1", 3000);
            Key key = new Key("test", "NestedMapOps", "key1");
            string binName = "NestedMap";
            string outKey = "Out";
            string innerKey = "Inner";
            Value value = Value.Get(99);

            try
            {
                WritePolicy wp = new WritePolicy()
                {
                    expiration = 3600,
                    recordExistsAction = RecordExistsAction.UPDATE,  //create or update
                    respondAllOps = true,
                    sendKey = true                    
                };
                var writeOps = GetNestedWriteOperationsForField<int>(binName, outKey, innerKey, value).ToArray();
                var recordWrite = client.Operate(wp, key, writeOps);

            }
            catch (AerospikeException ae) when (ae.Result == ResultCode.KEY_EXISTS_ERROR)
            {
                //ignore
            }

            var recordRead = client.Operate(null, key, GetNestedReadOperationsForField(binName, outKey, innerKey));

            Assert.NotNull(recordRead);
            Assert.Equal(99, recordRead.GetInt(binName));
        }

        private static IEnumerable<Operation> GetNestedWriteOperationsForField<TValue>(string binName, string outerKey, string innerKey, Value value)
        {

            ////create an empty innder map first as needed, otherwise AE exception, but won't work if insert for 2nd for same map key
            //yield return MapOperation.Put(new MapPolicy(MapOrder.UNORDERED, MapWriteMode.CREATE_ONLY), binName,
            //    Value.Get(outerKey), Value.Get(new Dictionary<string, TValue> { }));


            //Exp outerKeyExp = MapExp.GetByKey(MapReturnType.KEY, Exp.Type.STRING, Exp.Val(outerKey), Exp.Bin(binName, Exp.Type.MAP));

            //Exp outKeyNotExist = Exp.EQ(outerKeyExp, Exp.Val((string)null));

            //IDictionary innerEmptyMap = new Dictionary<string, TValue> { };

            //Exp insertMap = MapExp.Put(new MapPolicy(MapOrder.UNORDERED, MapWriteMode.CREATE_ONLY),
            //    Exp.Val(outerKey), Exp.Val(innerEmptyMap, MapOrder.UNORDERED), Exp.Bin(binName, Exp.Type.MAP));

            //Exp conditioned = Exp.Cond(outKeyNotExist, insertMap, Exp.Unknown());
            //yield return ExpOperation.Write(binName, Exp.Build(conditioned), ExpWriteFlags.CREATE_ONLY | ExpWriteFlags.EVAL_NO_FAIL | ExpWriteFlags.POLICY_NO_FAIL);

            //Above complex code is not working,  and they can all be replaced by CTX.MapKeyCreate

            yield return MapOperation.Increment(MapPolicy.Default, binName, new StringValue(innerKey), value, CTX.MapKeyCreate(Value.Get(outerKey), MapOrder.UNORDERED));
        }

        private static Operation GetNestedReadOperationsForField(string binName, string outerKey, string innerKey)
        {
            Exp condition = Exp.And(                
                Exp.BinExists(binName),  //Good  by itself or combine with Val(true) in Exp.And. Cannot be alone in And()
                Exp.NE(MapExp.GetByKey(MapReturnType.KEY, Exp.Type.STRING, Exp.Val(outerKey), Exp.Bin(binName, Exp.Type.MAP)), Exp.Val((string?)null))//works
             );

            //Exp outKeyNotExist = Exp.NE(MapExp.GetByKey(MapReturnType.KEY, Exp.Type.STRING, Exp.Val(outerKey), Exp.Bin(binName, Exp.Type.MAP)), Exp.Val((string?)null)); // works

            Exp work = MapExp.GetByKey(MapReturnType.VALUE, Exp.Type.INT, Exp.Val(innerKey), Exp.Bin(binName, Exp.Type.MAP), CTX.MapKey(Value.Get(outerKey))); //Good

            Exp conditionedWork = Exp.Cond(condition, work, Exp.Val(0)); // works
            return ExpOperation.Read(binName, Exp.Build(conditionedWork), ExpReadFlags.EVAL_NO_FAIL);         
        }



    }
}
