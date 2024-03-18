using System.Text.Json;
using Elasticsearch.Net;
using Nest;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace ElasticsearchCRUD
{
    class Program2
    {
        internal static async Task Main2()
        {
            CallLocal();
            //CallCloud();
            //await CallCloud2(); // works
        }

        internal static async Task CallCloud2()  //works
        {
            var cloudId = "eac8304211644a3a93436cd72165f9f3:dXMtY2VudHJhbDEuZ2NwLmNsb3VkLmVzLmlvOjQ0MyRhNjhjMzhiZWQwZGE0MmFlYTg1ZDU4NTRlYTJkMTFkZiQyOTcxOWFjN2FjMDE0ODU1YjMyNDBmOTI4ZThjODZkNg==";
            var apiKey = "bFB4MFNZNEJmcFE1cEpBMUplS1I6WDM5ck41Wi1RMFNrWlpMZHN4a0tGdw==";
            var client = new ElasticsearchClient(cloudId, new Elastic.Transport.ApiKey(apiKey));  //works

            var info = await client.InfoAsync();
            Console.WriteLine(info.ClusterName);
            Console.WriteLine(info.ApiCallDetails);

            var index = "ryan_index_callcloud2";
            var response = await client.Indices.CreateAsync(index);
            Console.WriteLine(response.ToString());

            var id = Guid.NewGuid().ToString();

            var doc = new YourDocumentModel
            {
                Id = id,
                Name = "Ryan Document from VS code CallCloud2",
                Description = "CRUD operations in Elasticsearch CallCloud2."
            };

            var responseDoc = await client.IndexAsync(doc, index);
            Console.WriteLine(response.ToString());

            var responseBack = await client.GetAsync<YourDocumentModel>(id, idx => idx.Index(index));

            if (responseBack.IsValidResponse)
            {
                var docBack = responseBack.Source;
                Console.WriteLine(docBack.Name);
            }
            else
            {
                Console.WriteLine(responseBack.ElasticsearchServerError);
            }

        }

        internal static void CallCloud()  //not work
        {
            var uri = new Uri("https://a68c38bed0da42aea85d5854ea2d11df.us-central1.gcp.cloud.es.io:443");
            var id = "ryanapikey2";
            var apiKey = "bFB4MFNZNEJmcFE1cEpBMUplS1I6WDM5ck41Wi1RMFNrWlpMZHN4a0tGdw==";  //not work, but curl works, CallCloud2 works as well

            var settings = new ConnectionSettings(uri)
                .ApiKeyAuthentication(id, apiKey)
                .DefaultIndex("ryanindex2"); // Set your default index here

            var client = new ElasticClient(settings);

            Exec(client);
        }

        internal static void CallLocal()
        {
            // Replace these values with your Elasticsearch endpoint, API key ID, and API key
            var uri = new Uri("https://localhost:9200");
            var id = "ryanusercreatedkey";   // both not work "ryanstackapikey";
            var apiKey = "MTFPdlNZNEJlSm5zY2YtZ3ZzeDY6STE3X1VvbWJUeENaS1RBVlpLdE9jUQ==";  //not work

            var settings = new ConnectionSettings(uri)               
                //.ApiKeyAuthentication(id, apiKey) never worked locally or in cloud
                .BasicAuthentication("ryanuser", "ryan_pwd")  //works
                .DefaultIndex("ryanindex1"); // Set your default index here

            var client = new ElasticClient(settings);
            Exec(client);
        }

        private static void Exec(ElasticClient client)
        {
            // Example: Perform a simple search
            var searchResponse = client.Search<object>(s => s
                .Query(q => q
                    .MatchAll()
                )
            );

            // Example: Output the results
            if (searchResponse.IsValid)
            {
                foreach (var hit in searchResponse.Hits)
                {
                    // Handle each hit
                    // For example:
                    Console.WriteLine(hit.Source);
                }
            }
            else
            {
                // Handle the case where the search response is not valid
                Console.WriteLine($"Error occurred: {searchResponse.ServerError.Error}");
            }
        }
    }
}