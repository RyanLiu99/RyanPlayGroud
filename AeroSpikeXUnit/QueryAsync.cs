using Aerospike.Client;
using AeroSpikeXUnit.Utils;
using Xunit.Abstractions;
using static Aerospike.Client.AsyncQueryValidate;
using static Aerospike.Client.Value;


namespace AeroSpikeXUnit;

public class QueryAsyncTest
{
    private readonly ITestOutputHelper _output;

    public QueryAsyncTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task TestQueryAsync()
    {
        try
        {
          
            var clientPolicy = new AsyncClientPolicy()
            {
                timeout = 1000,// milliseconds
            };

            using var client = new AsyncClient(clientPolicy, "10.0.0.95", 3000);


            QueryPolicy queryPolicy = new QueryPolicy()
            {
                maxConcurrentNodes = 10,
                compress = true

            };
            Statement statement = new Statement()
            {

                Filter = Filter.Equal("AccountId", "\"RAda1c20ad3e7e426fa87a08b42c92d08a\""),
                SetName = "Campaigns",
                Namespace = "ringba",
                BinNames = new string[] { "Id", "Name", "AccountId" }
                //Operations

            };

            var records = await client.QueryAsync(queryPolicy, statement);

            _output.WriteLine(records.Count.ToString());

        }
        catch (Exception ex)
        {
            _output.WriteLine(ex.ToString());
        }      
    }


}



