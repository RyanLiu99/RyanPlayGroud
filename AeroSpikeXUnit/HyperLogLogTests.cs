//using Aerospike.Client;
//using System;
//using Xunit;

//public class HyperLogLogTests : IDisposable
//{
//    private readonly AerospikeClient _client;
//    private readonly string _namespace = "test";
//    private readonly string _set = "hllSet";
//    private readonly Key _key;
//    private readonly string _hllBin = "hll_bin";

//    public HyperLogLogTests()
//    {
//        // Initialize the Aerospike client and key
//        _client = new AerospikeClient("127.0.0.1", 3000);
//        _key = new Key(_namespace, _set, "hllTestKey");
//    }

//    [Fact]
//    public void TestHyperLogLogOperations()
//    {
//        // Initialize an HLL object with precision of 10
//        var precision = 10;
//        _client.Put(null, _key, new Bin(_hllBin, HLLInit(precision)));

//        // Add elements to the HLL
//        var users = new[] { "user_1", "user_2", "user_3", "user_4", "user_5" };
//        foreach (var user in users)
//        {
//            _client.Operate(null, _key, Operation.HLLAdd(_hllBin, Value.Get(user)));
//        }

//        // Estimate the cardinality (number of distinct elements)
//        var result = _client.Operate(null, _key, Operation.HLLEstimate(_hllBin));
//        var estimatedCardinality = (long)result.GetLong(_hllBin);
//        Console.WriteLine($"Estimated Cardinality: {estimatedCardinality}");

//        // Assert that the estimated cardinality matches the number of added users
//        Assert.Equal(users.Length, estimatedCardinality);

//        // Add duplicate element and check the cardinality again (shouldn't increase)
//        _client.Operate(null, _key, Operation.HLLAdd(_hllBin, Value.Get("user_1")));
//        result = _client.Operate(null, _key, Operation.HLLEstimate(_hllBin));
//        var estimatedCardinalityAfterDuplicate = (long)result.GetLong(_hllBin);
//        Console.WriteLine($"Estimated Cardinality After Adding Duplicate: {estimatedCardinalityAfterDuplicate}");

//        Assert.Equal(users.Length, estimatedCardinalityAfterDuplicate);

//        // Merge with another HLL from a different record (assume another record has users user_6 and user_7)
//        var otherKey = new Key(_namespace, _set, "hllTestKey2");
//        _client.Put(null, otherKey, new Bin(_hllBin, HLLInit(precision)));
//        var otherUsers = new[] { "user_6", "user_7" };
//        foreach (var user in otherUsers)
//        {
//            _client.Operate(null, otherKey, Operation.HLLAdd(_hllBin, Value.Get(user)));
//        }

//        // Union HLL from both records
//        _client.Operate(null, _key, Operation.HLLUnion(_hllBin, otherKey, _hllBin));

//        // Recalculate the estimated cardinality after union
//        result = _client.Operate(null, _key, Operation.HLLEstimate(_hllBin));
//        var finalEstimatedCardinality = (long)result.GetLong(_hllBin);
//        Console.WriteLine($"Final Estimated Cardinality After Union: {finalEstimatedCardinality}");

//        // Assert that the final estimated cardinality matches the number of unique users in both sets
//        var totalUsers = users.Length + otherUsers.Length;
//        Assert.Equal(totalUsers, finalEstimatedCardinality);
//    }

//    [Fact]
//    public void TestHLLMayContain()
//    {
//        // Initialize an HLL object
//        var precision = 10;
//        _client.Put(null, _key, new Bin(_hllBin, HLLInit(precision)));

//        // Add users to the HLL
//        var users = new[] { "user_1", "user_2", "user_3" };
//        foreach (var user in users)
//        {
//            _client.Operate(null, _key, Operation.HLLAdd(_hllBin, Value.Get(user)));
//        }

//        // Check if HLL may contain a specific user
//        var result = _client.Operate(null, _key, Operation.HLLMayContain(_hllBin, Value.Get("user_2")));
//        var mayContainUser2 = (bool)result.GetBool(_hllBin);
//        Console.WriteLine($"HLL May Contain user_2: {mayContainUser2}");

//        // Assert the expected result
//        Assert.True(mayContainUser2);

//        // Check for a user not in the set
//        result = _client.Operate(null, _key, Operation.HLLMayContain(_hllBin, Value.Get("user_4")));
//        var mayContainUser4 = (bool)result.GetBool(_hllBin);
//        Console.WriteLine($"HLL May Contain user_4: {mayContainUser4}");

//        Assert.False(mayContainUser4);
//    }

//    // Clean up the test data after each run
//    public void Dispose()
//    {
//        _client.Delete(null, _key);
//        _client.Delete(null, new Key(_namespace, _set, "hllTestKey2"));
//        _client.Close();
//    }

//    // Helper method to initialize an HLL object
//    private static Value HLLInit(int precision)
//    {
//        return Value.Get(new object[] { precision });
//    }
//}
