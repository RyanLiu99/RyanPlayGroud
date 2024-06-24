using System;
using Aerospike.Client;
using Xunit.Abstractions;

namespace AeroSpikeXUnit
{
    public class TestRegEx
    {
        private readonly ITestOutputHelper _output;

        public TestRegEx(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void RegExTest()
        {
            // Configuration for the Aerospike client
            AerospikeClient client = new AerospikeClient("127.0.0.1", 3000);
            string namespaceName = "test";
            string setName = "demo";

            // Function to create records
            // CreateRecords(client, namespaceName, setName);

            // Define the regex pattern
            //string regexPattern = @"[,|:\[\]]";
            //string regexPattern = "[:,]";
            //string regexPattern = @"[],:|\[]"; //works
            string regexPattern = "[],:|[]"; //works

            // Define the expression
            Exp expression = Exp.RegexCompare(regexPattern, RegexFlag.NONE, Exp.StringBin("Name"));

            QueryPolicy qp = new QueryPolicy()
            {
                filterExp = Exp.Build(expression),
                includeBinData = true              
            };


            // Define and execute the query with the expression filter
            Statement stmt = new Statement()
            {
                Namespace = namespaceName,
                SetName = setName
            };
   

            RecordSet recordSet = client.Query(qp, stmt);
            var list = new List<string>();

            // Print the results
            try
            {
                while (recordSet.Next())
                {
                    var record = recordSet.Record;
                    var name = record.GetString("Name");
                    _output.WriteLine("Found: " + name );
                    list.Add(name);
                }
            }
            finally
            {
                recordSet.Close();
                client.Close();
            }

            Assert.Equal(3, list.Count);
        }

        static void CreateRecords(AerospikeClient client, string namespaceName, string setName)
        {
            string[] names = { "Alice", "Bob|Smith", "Charlie, Brown", "Dav:id", "Eve[Adams]" };
            for (int i = 0; i < names.Length; i++)
            {
                Key key = new Key(namespaceName, setName, i);
                Bin bin = new Bin("Name", names[i]);
                client.Put(null, key, bin);
            }
        }
    }
}
