using Aerospike.Client;
using Xunit.Abstractions;
using static Aerospike.Client.Value;


namespace AeroSpikeXUnit;

public class BinExistsTest
{
    private readonly ITestOutputHelper _output;

    private WritePolicy _writePolicy = new WritePolicy
    {
        expiration = -2, //seconds
        sendKey = false
    };

    public BinExistsTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void TestBinExists()
    {
        
        using var client = new AsyncClient( "127.0.0.1", 3000);

        WritePolicy wp = new WritePolicy() { sendKey =true, expiration=1000*600};

        // Create a new record
        var key = new Key("ryantest", "users", "testBinExist");
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
        client.Put(wp, key, bins);


    }

}

