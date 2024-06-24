using Aerospike.Client;
using Xunit.Abstractions;
using static Aerospike.Client.Value;


namespace AeroSpikeXUnit;

public class Curd
{
    private readonly ITestOutputHelper _output;

    private WritePolicy _writePolicy = new WritePolicy
    {
        expiration = -2, //seconds
        sendKey = true

    };

    public Curd(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void TestCURD()
    {
        
        using var client = new AsyncClient( "127.0.0.1", 3000);


        // // Create a new Aerospike _client
        // var config = new Config
        // {
        //     Hosts = new[] { new Host("127.0.0.1", 3000) }
        // };
        // var _client = new AerospikeClient(config);

        // Create a new record
        var key = new Key("ryantest", "users", "user1");
        var bins = new Bin[]
        {
            new Bin("name", "John Doe"),
            new Bin("email", "john.doe@example.com"),
            new Bin("age", 35),
            new Bin("address", new Dictionary<string, object>(){
                {"City", "Shanghai" },
                { "Street", "Park ave"},
                { "StNo", 152}
            })
        };
        client.Put(null, key, bins);

        // Read an existing record
        var record = client.Get(null, key);
        if (record != null)
        {
            _output.WriteLine($"Name: {record.GetString("name")}");
            _output.WriteLine($"Email: {record.GetString("email")}");
            _output.WriteLine($"Age: {record.GetInt("age")}");
            _output.WriteLine($"City: {record.GetMap("address")?["City"]}");
        }
        else
        {
            _output.WriteLine("Error reading record");
        }

        // Update an existing record
        bins = new Bin[]
        {
            new Bin("age", 36)
        };

        client.Put(null, key, bins);

        var readAgeBack = client.Get(null, key, "age");
        var ageBack = readAgeBack.GetInt("age");

        Assert.Equal(36, ageBack);

        // It does not provide default policy like _client.Get() does which has readPolicyDefault which is just new Policy()
        ReadCommand readCommand = new ReadCommand(client.Cluster, new QueryPolicy(), key); // here policy cannot be null, since it has no defaults
        readCommand.Execute();
        var ageBackeyCmd = readCommand.Record.GetInt("age");
        Assert.Equal(36, ageBackeyCmd);

        //// Delete a record
        //bool deleteResult = client.Delete(null, key);
        //_output.WriteLine($"Delete resutl is {deleteResult}");
        //Assert.True(deleteResult);

        var keys = new Key[] {
            new Key("ryantest", "users", "user1111"),
            new Key("ryantest", "users", "user2"),
            new Key("ryantest", "users", "user333"),
        };
        var records = client.Get(null, CancellationToken.None, keys, "age").GetAwaiter().GetResult();
        Assert.NotNull(records);
        Assert.Equal(3, records.Length);
        Assert.Null(records[0]);
        Assert.NotNull(records[1]);
        Assert.Null(records[2]);

    }

    [Fact]
    public void TestList()
    {
        var clientPolicy = new ClientPolicy();
        clientPolicy.timeout = 1000; // milliseconds
        using var client = new AerospikeClient(clientPolicy, "127.0.0.1", 3000);

        var key = new Key("ryantest", "users", "userList1");
        var listBinName = "myList";

        List<object> elements = new List<object>() { "write policy sendKey", true};
        //var listOp = ListOperation.Insert(listBinName, 0, new Value.ListValue(elements));
        var listOp = ListOperation.Insert(listBinName, 0, new Value.IntegerValue(33));
        var listOpBatch = ListOperation.AppendItems(listBinName,  elements);
       var record = client.Operate(_writePolicy, key, listOp, listOpBatch);
       var listBack = record.bins[listBinName] as IList<object>;

   

    }
}

