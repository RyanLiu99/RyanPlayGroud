using Aerospike.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroSpikeXUnit
{
    public  class MapInMapTest
    {
        string setName = "mapInMap";
        string namespaceName = "test";
        string userKey = "myKey1";

        [Fact]
        public void TestUpdateMapInMap()
        {
            CreateMapInMap();
            var result = UpdateMapInMap();
            Assert.Equal("updatedInnerValue1", result);
        }


        public void CreateMapInMap()
        {
            // Initialize Aerospike client
            var client = new AerospikeClient("127.0.0.1", 3000);

            // Define the namespace and set names
                        

            // Define the key for the record
            Key key = new Key(namespaceName, setName, userKey);

            // Create the inner map
            var innerMap = new Dictionary<string, string>
        {
            { "innerKey1", "innerValue1" },
            { "innerKey2", "innerValue2" }
        };

            // Create the outer map with the inner map as a value
            var outerMap = new Dictionary<string, object>
        {
            { "outerKey1", innerMap }
        };

            // Create a bin with the outer map
            var bin = new Bin("outerMapBin", outerMap);

            // Write the record with the nested map
            client.Put(null, key, bin, new Bin("key", userKey));

            // Print confirmation
            Console.WriteLine("Record with nested map created.");

            // Close the client connection
            client.Close();
        }

        public string? UpdateMapInMap()
        {
            var client = new AerospikeClient("127.0.0.1", 3000);


            // Define the key for the record
            Key key = new Key(namespaceName, setName, userKey);

            // Define the operation to update an entry in the inner map
            var operations = new Operation[]
            {
            MapOperation.Put(
                    new MapPolicy(),
                    "outerMapBin",
                    Value.Get("innerKey1"),  // Key for the inner map entry to update
                    Value.Get("updatedInnerValue1"),  // New value for the inner map entry
                    CTX.MapKey(Value.Get("outerKey1"))  // Context to specify the inner map
                )
            };

            // Perform the update operation
            client.Operate(null, key, operations);

            // Verify the update
            var record = client.Get(null, key);
            var updatedOuterMap = record.GetMap("outerMapBin");
            var updatedInnerMap = updatedOuterMap["outerKey1"] as System.Collections.IDictionary;

            Console.WriteLine("Updated inner map:");
        

            // Close the client connection
            client.Close();

            return updatedInnerMap?["innerKey1"]?.ToString();
        }
    }
}
