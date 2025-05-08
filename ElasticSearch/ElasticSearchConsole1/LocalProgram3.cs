using ElasticSearchConsole1;
using Nest;
using System;

public class MyDocument
{
    public string Id { get; set; }
    public string Title { get; set; }
    public DateTime Timestamp { get; set; }
}

public class LocalProgram3
{
    private static ElasticClient CreateClient()
    {
        var settings = new ConnectionSettings(new Uri("http://localhost:9207/"))            
            .DefaultIndex("my-index");

        return new ElasticClient(settings);
    }

    public static async Task Run()
    {
        var client = CreateClient();

        // 1. Index a document
        var doc = new MyDocument
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Hello OpenSearch!",
            Timestamp = DateTime.UtcNow
        };

        IndexResponse indexResponse = await client.IndexDocumentAsync(doc);

        if (!indexResponse.IsValid)
        {
            Console.WriteLine("Error indexing document: " + indexResponse.GetErrorInfo());
        }

        Console.WriteLine("Document indexed with ID: " + doc.Id);

        // 2. Get the document back
        var getResponse = client.Get<MyDocument>(doc.Id);

        if (getResponse.Found)
        {
            Console.WriteLine("Read back: " + getResponse.Source.Title);
        }
        else
        {
            Console.WriteLine("Document not found!" + getResponse.GetErrorInfo());
        }

        //3 invlid query
        var responseBad = client.Search<MyDocument>(s => s
            .Index("xyz-index")
            .Query(q => q.Match(m => m.Field("xyz_field").Query("test")))
        );

        if (responseBad.IsValid)
        {
            Console.WriteLine("Read back bad one! How come!" );
        }
        else
        {
            Console.WriteLine("Bad as expected! ErrorInfo I got:" + responseBad.GetErrorInfo());
        }
    }
}
