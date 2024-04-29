using Aerospike.Client;
using Xunit.Abstractions;

namespace AeroSpikeXUnit;

public class UnitTest1
{
     private readonly ITestOutputHelper _output;
     
public UnitTest1(ITestOutputHelper output)
{
    _output = output;
}

    [Fact]
    public void Test1()
    {
        var clientPolicy = new ClientPolicy();
        clientPolicy.timeout = 1000; // milliseconds

        using var client = new AerospikeClient(clientPolicy, "127.0.0.1", 3000);


        // // Create a new Aerospike client
        // var config = new Config
        // {
        //     Hosts = new[] { new Host("127.0.0.1", 3000) }
        // };
        // var client = new AerospikeClient(config);

        // Create a new record
        var key = new Key("ryantest", "users", "user1");
        var bins = new Bin[]
        {
            new Bin("name", "John Doe"),
            new Bin("email", "john.doe@example.com"),
            new Bin("age", 35)
        };
        client.Put(null, key, bins);

        // Read an existing record
        var record = client.Get(null, key);
        if (record != null)
        {
            _output.WriteLine($"Name: {record.GetString("name")}");
            _output.WriteLine($"Email: {record.GetString("email")}");
            _output.WriteLine($"Age: {record.GetInt("age")}");
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

        // Delete a record
        // bool deleteResult= client.Delete(null, key);
        //_output.WriteLine($"Delete resutl is {deleteResult}");

    }
}
